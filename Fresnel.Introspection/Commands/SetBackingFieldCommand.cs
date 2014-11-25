using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class SetBackingFieldCommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;

        public SetBackingFieldCommand
        (
            RealTypeResolver realTypeResolver,
            TemplateCache templateCache
        )
        {
            _RealTypeResolver = realTypeResolver;
            _TemplateCache = templateCache;
        }

        public void Invoke(object obj, string propertyName, object value)
        {
            var realType = _RealTypeResolver.GetRealType(obj.GetType());
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(realType);
            this.Invoke(tClass, obj, propertyName, value);
        }

        /// <summary>
        /// Sets the value of the _backing field_ for the given property
        /// </summary>
        public void Invoke(ClassTemplate tClass, object obj, string propertyName, object value)
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

            tProp.SetField(obj, value);
        }

    }
}
