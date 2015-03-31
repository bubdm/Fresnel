using Newtonsoft.Json;
using T4TS;
namespace Envivo.Fresnel.UiCore.Model.TypeInfo
{
    [TypeScriptInterface]
    public class EnumItemVM : BaseViewModel
    {
        public string EnumName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public int Value { get; set; }
    }
}