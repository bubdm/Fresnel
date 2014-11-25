using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class InvokeMethodCommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;

        public InvokeMethodCommand
        (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
        }

        public void Invoke(object obj, string methodName, IEnumerable<object> args)
        {
            var realType = _RealTypeResolver.GetRealType(obj.GetType());
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(realType);
            this.Invoke(tClass, obj, methodName, args);
        }

        /// <summary>
        /// Invokes the method on the given object, using the provided arguments
        /// </summary>
        public object Invoke(ClassTemplate tClass, object obj, string methodName, IEnumerable<object> args)
        {
            if (tClass == null)
                throw new ArgumentNullException("tClass");

            if (obj == null)
                throw new ArgumentNullException("obj");

            if (methodName == null)
                throw new ArgumentNullException("methodName");

            var tMethod = tClass.Methods[methodName];
            if (tMethod == null)
            {
                var msg = string.Concat("Cannot determine ", tClass.Name, ".", methodName);
                throw new ArgumentException(msg);
            }

            var result = tMethod.Invoke(obj, args);
            return result;
        }

    }
}
