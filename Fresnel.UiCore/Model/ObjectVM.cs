using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class ObjectVM : BaseViewModel
    {
        public Guid ID { get; set; }

        public string Type { get; set; }

        public IEnumerable<PropertyVM> Properties { get; set; }

        public IEnumerable<MethodVM> Methods { get; set; }

        /// <summary>
        /// Determines if the Object can be saved to a data store
        /// </summary>
        public bool IsPersistable { get; set; }

        public DirtyStateVM DirtyState { get; set; }
    }
}