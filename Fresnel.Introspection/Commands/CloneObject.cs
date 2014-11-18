using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Introspection.Commands
{
    public class CloneObject
    {
        private readonly object[] _EmptyCtorArgs = new object[0];

        private TemplateCache _TemplateCache;


        public CloneObject(TemplateCache templateCache)
        {
            _TemplateCache = templateCache;
        }

        /// <summary>
        /// Returns a shallow copy of the given object. If the source object implements ICloneable, it will use that to get a clone. Otherwise, a clone will be created using brute force.
        /// </summary>
        
        public object Clone(object source, bool withForce)
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

        /// <summary>
        /// Creates a shallow clone using brute force reflection
        /// </summary>
        /// <param name="source"></param>
        
        private object CreateClone(ClassTemplate tClass, object source)
        {
            var result = tClass.DefaultConstructor.Invoke(_EmptyCtorArgs);

            foreach (var tProp in tClass.Properties.Values)
            {
                // Two (or more) Domain Objects must not share the same list (they can share the list's elements):
                if (tProp.IsCollection)
                    continue;

                var value = tProp.GetField(source) ?? tProp.GetProperty(source);

                // Some properties might be value objects, in which case they should be cloned as well:
                var cloneable = value as ICloneable;
                if (cloneable != null)
                {
                    value = cloneable.Clone();
                }

                // Don't attempt copying values if the target doesn't allow it:
                if (tProp.BackingField != null || tProp.PropertyInfo.CanWrite)
                {
                    tProp.SetField(result, value);
                }
            }

            return result;
        }

    }
}
