using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class SetPropertyCommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;

        public SetPropertyCommand
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
            var realType = _RealTypeResolver.GetRealType(obj);
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(realType);

            this.Invoke(tClass, obj, propertyName, value);
        }
        
        /// <summary>
        /// Sets the value of the property on the given object
        /// </summary>
        public void Invoke(ClassTemplate tClass, object obj, string propertyName, object value)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");

            var tProp = tClass.Properties[propertyName];
            if (tProp == null)
            {
                var msg = string.Concat("Cannot determine ", tClass.Name, ".", propertyName);
                throw new ArgumentException(msg);
            }

            this.Invoke(tClass, obj, tProp, value);
        }

        /// <summary>
        /// Sets the value of the property on the given object
        /// </summary>
        public void Invoke(ClassTemplate tClass, object obj, PropertyTemplate tProperty, object value)
        {
            if (tClass == null)
                throw new ArgumentNullException("tClass");

            if (obj == null)
                throw new ArgumentNullException("obj");

            if (tProperty == null)
                throw new ArgumentNullException("tProperty");

            tProperty.SetProperty(obj, value);
        }
    }
}