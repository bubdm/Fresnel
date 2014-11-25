using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class FieldInfoMap : ReadOnlyDictionary<string, FieldInfo>
    {
        /// <summary>
        /// All fields that are used for Event handling
        /// </summary>
        public IDictionary<string, FieldInfo> EventDelegates { get; private set; }

        public FieldInfoMap()
        {
            this.EventDelegates = new Dictionary<string, FieldInfo>();
        }

        public FieldInfoMap
        (
            IDictionary<string, FieldInfo> items,
            IDictionary<string, FieldInfo> eventDelegates
        )
            : base(items)
        {
            this.EventDelegates = eventDelegates;
        }
    }

}
