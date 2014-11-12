using System;

namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for an Enum Property
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class EnumAttribute : PropertyAttribute
    {

        /// <summary>
        /// The preferred control for viewing and editing the enum value
        /// </summary>
        public EnumEditorControl PreferredUiControl { get; set; }

        /// <summary>
        /// The type of Query Specification that is used to restrict the items shown in the UI
        /// </summary>
        public Type ItemFilter { get; set; }
    }

    [Serializable()]
    public enum EnumEditorControl
    {
        DropDownList,
        RadioOptions,
        Slider,
    }
}
