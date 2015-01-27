﻿using Envivo.Fresnel.Configuration;
using T4TS;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    [TypeScriptInterface]
    public class DateTimeVM : ITypeInfo
    {
        public string Name { get; set; }

        public InputControlTypes PreferredControl { get; set; }

        public string CustomFormat { get; set; }
    }
}