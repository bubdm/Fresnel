using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class GetPropertyCommand
    {
        private RealTypeResolver _RealTypeResolver;
        private TemplateCache _TemplateCache;

        public GetPropertyCommand
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
            var realType = _RealTypeResolver.GetRealType(obj.GetType());
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(realType);
            var result = this.Invoke(tClass, obj, propertyName);
            return result;
        }

        /// <summary>
        /// Returns the value of the property on the given object
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

            var value = tProp.GetProperty(obj);
            return value;
        }

    }
}
