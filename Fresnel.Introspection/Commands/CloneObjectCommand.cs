using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class CloneObjectCommand
    {
        private readonly object[] _EmptyCtorArgs = new object[0];

        private TemplateCache _TemplateCache;

        public CloneObjectCommand(TemplateCache templateCache)
        {
            _TemplateCache = templateCache;
        }

        /// <summary>
        /// Returns a shallow copy of the given object. If the source object implements ICloneable, it will use that to get a clone. Otherwise, a clone will be created using brute force.
        /// </summary>
        public object Invoke(object source, bool withForce)
        {
            var realType = source.GetType();
            var tClass = (ClassTemplate)_TemplateCache.GetTemplate(realType);

            //if (!source.GetRealType().IsDerivedFrom(this.RealObjectType))
            //{
            //    throw new ArgumentException("Source is a " + source.GetRealType().Name + ", but this template is for " + this.RealObjectType.Name);
            //}

            object result = null;

            if (!withForce)
            {
                var cloneable = source as ICloneable;
                if (cloneable != null)
                {
                    result = cloneable.Clone();
                }
            }

            if (result == null || withForce)
            {
                result = this.CreateClone(tClass, source);
            }

            return result;
        }

        private object CreateClone(ClassTemplate tClass, object source)
        {
            var clone = tClass.CreateInstance();

            foreach (var tProp in tClass.Properties.Values)
            {
                // Use the backing fields when possible,
                // to prevent proxies from triggering lazy-loads unnecessarily:
                var value = tProp.BackingField != null ?
                            tProp.GetField(source) :
                            tProp.GetProperty(source);

                if (tProp.BackingField != null)
                {
                    tProp.SetField(clone, value);
                }
                else
                {
                    tProp.SetProperty(clone, value);
                }
            }

            return clone;
        }
    }
}