using Envivo.Fresnel.Introspection.Templates;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class CreateObjectCommand
    {
        private RealTypeResolver _RealTypeResolver;

        public CreateObjectCommand
        (
            RealTypeResolver realTypeResolver
        )
        {
            _RealTypeResolver = realTypeResolver;
        }

        /// <summary>
        /// Creates and returns an instance of the Object, using any zero-arg constructor (including internal/protected/private)
        /// </summary>
        public object Invoke(ClassTemplate tClass)
        {
            if (tClass.HasDefaultConstructor)
            {
                var newInstance = tClass.CreateInstance();
                return newInstance;
            }

            throw new FresnelException(tClass.Name + " cannot be created");
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
                throw new FresnelException(tClass.Name + " cannot be created");
            }

            object result = null;

            if (constructorArg != null)
            {
                var realType = _RealTypeResolver.GetRealType(constructorArg.GetType());

                if (tClass.CanBeCreatedWith(realType))
                {
                    object[] args = { constructorArg };
                    result = tClass.RealType.Assembly.CreateInstance(tClass.RealType.FullName, true, BindingFlags.CreateInstance, null, args, null, null);
                }
            }

            // If we haven't got an object, try using the zero-arg constructor:
            if (result == null)
            {
                result = tClass.CreateInstance();
            }

            return result;
        }
    }
}
