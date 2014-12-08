using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Editing
{
    public enum EditorType
    {
        Unknown,
        None,
        Boolean,
        Date,
        Time,
        DateTime,
        EnumCheckboxes,
        EnumRadioOptions,
        EnumSlider,
        EnumDropDown,
        Integer,
        IntegerSlider,
        Number,
        NumberSlider,
        Currency,
        String,
        Character,
        MultiLineText,
        Password,
        MaskedString,
        MarkdownText,
        Hyperlink,
        FileDialog,
        ObjectSelectionList,
        InlineObject,
        InlineCollection
    }
}
