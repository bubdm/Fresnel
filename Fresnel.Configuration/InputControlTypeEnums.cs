using Envivo.Fresnel.DomainTypes.Interfaces;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    public enum InputControlTypes
    {
        // These map to HTML5 field input types (mostly!)
        None,
        Text,
        TextArea,
        RichTextArea,
        Password,
        Email,
        Search,
        Telephone,
        Url,
        Radio,
        Checkbox,
        Date,
        DateTimeLocal,
        Time,
        Month,
        Week,
        Color,
        Number,
        Currency,
        Range,
        File,
        Select
    }

}
