using Envivo.Fresnel.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// This is a simple list of options.
    /// This enum is not defined inside a class.
    /// </summary>
    [Flags]
    public enum CombinationOptions
    {
        /// <summary>
        /// This is the value Eggs
        /// </summary>
        Eggs = 1,

        /// <summary>
        /// This is the value Ham
        /// </summary>
        Ham = 2,

        /// <summary>
        /// This is the value Cheese
        /// </summary>
        Cheese = 4,
    }

    /// <summary>
    /// A set of Enum properties
    /// </summary>
    public class EnumValues
    {
        /// <summary>
        /// This is a simple list of options.
        /// This enum is defined inside another class.
        /// </summary>
        public enum IndividualOptions
        {
            /// <summary>
            /// This is the value NONE
            /// </summary>
            None = 0,

            /// <summary>
            /// This is the value RED
            /// </summary>
            Red = 10,

            /// <summary>
            /// This is the value GREEN
            /// </summary>
            Green = 20,

            /// <summary>
            /// This is the value BLUE
            /// </summary>
            Blue = 30
        }

        private IndividualOptions _EnumValue = IndividualOptions.None;

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// This enum should be shown as a drop-down list.
        /// </summary>
        public IndividualOptions EnumValue
        {
            get { return _EnumValue; }
            set { _EnumValue = value; }
        }

        private CombinationOptions _EnumSwitches = 0;

        /// <summary>
        /// This enum should be shown as a multi-choice check-list.
        /// </summary>
        public CombinationOptions EnumSwitches
        {
            get { return _EnumSwitches; }
            set { _EnumSwitches = value; }
        }

        /// <summary>
        /// This enum should be shown as a drop-down list.
        /// The items are restricted (at run-time) using EnumValuesFilterSpecification
        /// </summary>
        [Display(GroupName = "Enum presentation")]
        [FilterQuerySpecification(typeof(EnumValuesFilterSpecification))]
        public IndividualOptions EnumValueDropDown
        {
            get { return _EnumValue; }
            set { _EnumValue = value; }
        }

        /// <summary>
        /// This enum should be shown as a set of Radio Options
        /// </summary>
        [Display(GroupName = "Enum presentation")]
        [UiControlHint(UiControlType.Radio)]
        public IndividualOptions EnumRadioOptions
        {
            get { return _EnumValue; }
            set { _EnumValue = value; }
        }
    }
}