using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class CreateObjectCommand
    {
        private IDomainObjectFactory _DomainObjectFactory;
        private RealTypeResolver _RealTypeResolver;

        public CreateObjectCommand
        (
            IDomainObjectFactory domainObjectFactory,
            RealTypeResolver realTypeResolver
        )
        {
            _DomainObjectFactory = domainObjectFactory;
            _RealTypeResolver = realTypeResolver;
        }

        /// <summary>
        /// Creates and returns an instance of the object
        /// </summary>
        public object Invoke(ClassTemplate tClass)
        {
            return this.Invoke(tClass, null);
        }

        /// <summary>
        /// Creates and returns an instance of the Object, using a constructor which accepts the given argument
        /// </summary>
        /// <param name="constructorArg">The argument to pass to the constructor</param>
        /// <returns>A new instance of the Object</returns>

        public object Invoke(ClassTemplate tClass, object constructorArg)
        {
            if (!tClass.IsCreatable)
            {
                throw new IntrospectionException(tClass.Name + " cannot be created");
            }

            // Try using the IoC container:
            var result = this.CreateObjectUsingIoCFactory(tClass, constructorArg);

            if (result == null && constructorArg != null)
            {
                var realType = _RealTypeResolver.GetRealType(constructorArg);

                if (tClass.CanBeCreatedWith(realType))
                {
                    object[] args = { constructorArg };
                    result = tClass.RealType.Assembly.CreateInstance(tClass.RealType.FullName, true, BindingFlags.CreateInstance, null, args, null, null);
                }
            }

            if (result == null)
            {
                // Try using the zero-arg constructor:
                result = tClass.CreateInstance();
            }

            if (result == null)
            {
                throw new IntrospectionException("Cannot find a way to create " + tClass.FriendlyName);
            }

            return result;
        }

        private object CreateObjectUsingIoCFactory(ClassTemplate tClass, object constructorArg)
        {
            if (constructorArg == null)
            {
                var result = _DomainObjectFactory.Create(tClass.RealType);
                return result;
            }
            else
            {
                var result = _DomainObjectFactory.Create(tClass.RealType, constructorArg);
                return result;
            }
        }
    }
}