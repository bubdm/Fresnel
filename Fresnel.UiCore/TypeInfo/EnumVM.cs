using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class EnumVM : ITypeInfo
    {
        public string Name
        {
            get { return "enum"; }
        }

        /// <summary>
        /// The preferred control for viewing and editing the enum value
        /// </summary>
        public EnumEditorControl PreferredUiControl { get; set; }

    }
}
