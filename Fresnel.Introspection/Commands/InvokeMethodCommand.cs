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

        public object Invoke(object obj, string methodName, IEnumerable<object> args)
        {
            var realType = _RealTypeResolver.GetRealType(obj);
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(realType);
            var tMethod = tClass.Methods[methodName];
            var result = this.Invoke(obj, tMethod, args);
            return result;
        }

        /// <summary>
        /// Invokes the method on the given object, using the provided arguments
        /// </summary>
        public object Invoke(object obj, MethodTemplate tMethod, IEnumerable<object> args)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (tMethod == null)
                throw new ArgumentException("tMethod");

            var result = tMethod.Invoke(obj, args);
            return result;
        }
    }
}