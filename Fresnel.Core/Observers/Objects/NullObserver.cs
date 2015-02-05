using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;
using System;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// An Observer which represents a null Object. Used to implement the "Null Object" pattern
    /// </summary>
    public class NullObserver : BaseObjectObserver
    {
        public NullObserver
            (
            Type objectType,
            BaseClassTemplate tClass
            )
            : base(null, objectType, tClass)
        {
        }

        [JsonIgnore]
        public new ClassTemplate Template
        {
            get { return (ClassTemplate)base.Template; }
        }
    }
}