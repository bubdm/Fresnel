using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class GetBackingFieldCommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;

        public GetBackingFieldCommand
        (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
        }

        public object Invoke(object obj, string propertyName)
        {
            var realType = _RealTypeResolver.GetRealType(obj);
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(realType);
            var result = this.Invoke(tClass, obj, propertyName);
            return result;
        }

        /// <summary>
        /// Returns the value of the _backing field_ of the given property
        /// </summary>
        public object Invoke(ClassTemplate tClass, object obj, string propertyName)
        {
            if (tClass == null)
                throw new ArgumentNullException("tClass");

            if (obj == null)
                throw new ArgumentNullException("obj");

            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            var tProp = tClass.Properties[propertyName];
            if (tProp == null)
            {
                var msg = string.Concat("Cannot determine ", tClass.Name, ".", propertyName);
                throw new ArgumentException(msg);
            }

            if (tProp.BackingField == null)
            {
                var msg = string.Concat("Cannot determine backing field ", tClass.Name, ".", propertyName);
                throw new ArgumentException(msg);
            }

            var value = tProp.GetField(obj);
            return value;
        }
    }
}