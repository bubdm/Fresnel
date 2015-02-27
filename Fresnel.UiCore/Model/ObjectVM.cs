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

        public IEnumerable<SettableMemberVM> Properties { get; set; }

        public IEnumerable<MethodVM> Methods { get; set; }

        /// <summary>
        /// Determines if the Object is brand new
        /// </summary>
        public bool IsTransient { get; set; }

        /// <summary>
        /// Determines if the Object exists in the data store
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// Determines if the Object has been modified
        /// </summary>
        public bool IsModified { get; set; }
        
    }
}