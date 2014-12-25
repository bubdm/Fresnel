using Envivo.Fresnel.Configuration;
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
            ClassTemplate tClass
            )
            : base(null, objectType, tClass)
        {

        }

        [JsonIgnore]
        public ClassTemplate Template
        {
            get { return (ClassTemplate)base.Template; }
        }

    }
}
