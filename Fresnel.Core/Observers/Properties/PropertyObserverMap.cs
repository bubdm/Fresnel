using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection;

namespace Envivo.Fresnel.Core.Observers
{

    public class PropertyObserverMap : ReadOnlyDictionary<string, BasePropertyObserver>
    {
        private IEnumerable<ObjectPropertyObserver> _ForObjects;

        public PropertyObserverMap()
            : base()
        {
            _ForObjects = new List<ObjectPropertyObserver>();
        }

        public PropertyObserverMap(IDictionary<string, BasePropertyObserver> items)
            : base(items)
        {
            _ForObjects = items.Values.OfType<ObjectPropertyObserver>().ToList();
        }

        internal IEnumerable<ObjectPropertyObserver> ForObjects
        {
            get { return _ForObjects; }
        }

        internal BasePropertyObserver this[MethodInfo methodInfo]
        {
            get { return this[GetPropertyName(methodInfo)]; }
        }

        private string GetPropertyName(MethodInfo methodInfo)
        {
            // This is a property getter or setter (ie "get_XXX" or "set_XXX")
            return (methodInfo.IsSpecialName) ?
                    methodInfo.Name.Substring(4) :
                    null;
        }

        internal void ResetLazyLoadStatus(bool isOuterClassPersistable)
        {
            if (_ForObjects == null)
                return;

            foreach (var oProp in _ForObjects)
            {
                oProp.ResetLazyLoadStatus(isOuterClassPersistable);
            }
        }

    }

}
