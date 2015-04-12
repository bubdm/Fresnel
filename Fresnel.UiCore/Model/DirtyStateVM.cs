using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Newtonsoft.Json;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class DirtyStateVM
    {
        /// <summary>
        /// The ID of the associated Object
        /// </summary>
        public Guid ObjectID { get; set; }

        /// <summary>
        /// Determines if the Object is brand new
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsTransient { get; set; }

        /// <summary>
        /// Determines if the Object exists in the data store
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsPersistent { get; set; }

        /// <summary>
        /// Determines if the Object has been modified
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool IsDirty { get; set; }

        /// <summary>
        /// Determines if the Object's children have been modified
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool HasDirtyChildren { get; set; }

        public override string ToString()
        {
            return string.Concat(this.IsTransient ? "N " : " ",
                                 this.IsPersistent ? "P " : " ",
                                 this.IsDirty ? "D " : " ",
                                 this.HasDirtyChildren ? "DC " : " ");
        }
    }
}