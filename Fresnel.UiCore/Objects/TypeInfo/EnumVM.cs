using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects.TypeInfo
{
    public class EnumVM : ITypeInfo
    {
        /// <summary>
        /// The preferred control for viewing and editing the enum value
        /// </summary>
        public EnumEditorControl PreferredUiControl { get; set; }

    }
}
