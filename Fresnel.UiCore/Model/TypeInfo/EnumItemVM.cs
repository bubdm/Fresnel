using System.ComponentModel;
using T4TS;
namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    [TypeScriptInterface]
    public class EnumItemVM : BaseViewModel
    {
        public string EnumName { get; set; }

        public int Value { get; set; }

        [DefaultValue(false)]
        public bool IsChecked { get; set; }
    }
}