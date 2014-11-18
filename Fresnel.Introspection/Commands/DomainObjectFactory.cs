using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class DomainObjectFactory
    {
        private RealTypeResolver _RealTypeResolver;

        public DomainObjectFactory
        (
            RealTypeResolver realTypeResolver
        )
        {
            _RealTypeResolver = realTypeResolver;
        }

        /// <summary>
        /// Creates and returns an instance of the Object
        /// </summary>
        /// <returns>A new instance of the Object</returns>
        
        public object CreateDomainObject(ClassTemplate tClass)
        {
            // TODO: Add support for DynamicMethod constructors

            // Delegate to the overloaded method:
            return this.CreateDomainObject(null);
        }

        /// <summary>
        /// Creates and returns an instance of the Object, using any zero-arg constructor (including internal/protected/private)
        /// </summary>
        
        public object CreateDomainObjectByForce(ClassTemplate tClass)
        {
            if (tClass.HasDefaultConstructor)
            {
                // TODO: Use DynamicMethod instead
                var newInstance = tClass.RealObjectType.Assembly.CreateInstance(tClass.RealObjectType.FullName, true);
                return newInstance;
            }
            //else if (tClass.DefaultConstructor == null)
            //{
            //    var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
            //    _ZeroArgConstructor = this.RealObjectType.GetConstructor(bindingFlags, null, new Type[0], null);
            //}

            if (tClass.DefaultConstructor != null)
            {
                var newInstance = tClass.DefaultConstructor.Invoke(new object[0]);
                return newInstance;
            }

            throw new FresnelException(tClass.Name + " cannot be created");
        }

        /// <summary>
        /// Creates and returns an instance of the Object, using a constructor which accepts the given argument
        /// </summary>
        /// <param name="constructorArg">The argument to pass to the constructor</param>
        /// <returns>A new instance of the Object</returns>
        
        public object CreateDomainObject(ClassTemplate tClass, object constructorArg)
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
                    result = tClass.RealObjectType.Assembly.CreateInstance(tClass.RealObjectType.FullName, true, BindingFlags.CreateInstance, null, args, null, null);
                }
            }

            // If we haven't got an object, try using the default constructor:
            if (result == null)
            {
                result = tClass.RealObjectType.Assembly.CreateInstance(tClass.RealObjectType.FullName, true);
            }

            return result;
        }
    }
}
