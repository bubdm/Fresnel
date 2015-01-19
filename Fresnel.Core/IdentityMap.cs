using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core
{
    /// <summary>
    /// A dictionary of POCO objects keyed by GUID
    /// </summary>
    public class IdentityMap : Dictionary<Guid, object>
    {
        private Dictionary<object, Guid> _InverseMap = new Dictionary<object, Guid>();

        public new void Add(Guid key, object value)
        {
            base.Add(key, value);
            _InverseMap[value] = key;
        }

        public Guid GetIdFor(object value)
        {
            return _InverseMap.TryGetValueOrDefault(value, Guid.Empty);
        }
    }
}