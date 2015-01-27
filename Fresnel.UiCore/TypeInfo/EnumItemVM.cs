﻿using T4TS;
namespace Envivo.Fresnel.UiCore.TypeInfo
{
    [TypeScriptInterface]
    public class EnumItemVM : BaseViewModel
    {
        public string EnumName { get; set; }

        public int Value { get; set; }

        public bool IsChecked { get; set; }
    }
}