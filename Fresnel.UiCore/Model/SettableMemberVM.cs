using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    /// <summary>
    /// Represents a Property or Parameter Value in the UI
    /// </summary>
    [TypeScriptInterface]
    public class SettableMemberVM : BaseViewModel
    {
        public Guid? ObjectID { get; set; }

        [DefaultValue(-1)]
        public int Index { get; set; }

        public string InternalName { get; set; }

        /// <summary>
        /// The Types of objects that are allowed for this member
        /// </summary>
        public string[] AllowedClassTypes { get; set; }

        [DefaultValue(false)]
        public bool IsRequired { get; set; }

        [DefaultValue(false)]
        public bool IsLoaded { get; set; }

        [DefaultValue(false)]
        public bool IsObject { get; set; }

        [DefaultValue(false)]
        public bool IsCollection { get; set; }

        [DefaultValue(false)]
        public bool IsNonReference { get; set; }

        public ITypeInfo Info { get; set; }

        public ValueStateVM State { get; set; }

        public override string ToString()
        {
            return string.Concat(this.InternalName, "/", this.State);
        }
    }
}