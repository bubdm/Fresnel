using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;
using System;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// An observer for an Enumeration value
    /// </summary>
    public class EnumObserver : NonReferenceObserver
    {
        public EnumObserver(object enumValue, Type enumType, EnumTemplate tEnum)
            : base(enumValue, enumType, tEnum)
        {
        }

        [JsonIgnore]
        public new EnumTemplate Template
        {
            get { return (EnumTemplate)base.Template; }
        }

        ///// <summary>
        ///// The underlying Template used to create this Observer
        ///// </summary>

        //public EnumTemplate EnumTemplate
        //{
        //    get { return (EnumTemplate)base.Template; }
        //}

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="showStatus"></param>
        ///// <param name="showName"></param>
        ///// <param name="showSummary"></param>

        ///// <remarks>This method must be as optimal as possible, as it will be called many times in rapid succession</remarks>
        //public override string ToString(bool showStatus, bool showName, bool showSummary)
        //{
        //    var sb = new StringBuilder();

        //    var canShowName = showName && (this.OuterMember != null);
        //    if (canShowName)
        //    {
        //        sb.Append(this.OuterMember.FriendlyName);
        //    }

        //    if (showSummary)
        //    {
        //        string value = GetFormattedValue();
        //        if (value.IsNotEmpty())
        //        {
        //            if (canShowName)
        //            {
        //                sb.Append("=");
        //            }
        //            sb.Append(value);
        //        }
        //    }

        //    return sb.ToString();
        //}

        ///// <summary>
        ///// Used to format the Object's value into a displayable format.
        ///// The format is dependent on the Attribute declarations applied to the Object and it's members.
        ///// </summary>
        ///// <returns>The formatted value as a String</returns>

        //internal string GetFormattedValue()
        //{
        //    if (this.RealObject == null)
        //        return string.Empty;

        //    // Use the local regional settings to format the value
        //    if (this.Template.IsBitwiseEnum)
        //    {
        //        return formattedBitwiseEnum();
        //    }
        //    else
        //    {
        //        return formattedEnum();
        //    }
        //}

        //private string formattedEnum()
        //{
        //    var key = this.RealObject.ToString();
        //    var tEnumItem = this.Template.EnumItems.TryGetValueOrNull(key);

        //    if (tEnumItem != null)
        //    {
        //        return tEnumItem.FriendlyName;
        //    }
        //    else
        //    {
        //        return "<enum value is invalid>";
        //    }
        //}

        //private string formattedBitwiseEnum()
        //{
        //    var value = (int)this.RealObject;

        //    // Represent the selection as a delimited list:
        //    var selectedEnumItems = new List<string>();
        //    foreach (var tEnumItem in this.Template.EnumItems.Values)
        //    {
        //        // Do a bitwise test:
        //        var enumValue = (int)tEnumItem.Value;
        //        var isBitSet = (value & enumValue) == enumValue;

        //        if (isBitSet)
        //        {
        //            selectedEnumItems.Add(tEnumItem.FriendlyName);
        //        }
        //    }

        //    var result = (string[])Array.CreateInstance(typeof(string), selectedEnumItems.Count);
        //    selectedEnumItems.CopyTo(result);
        //    return string.Format("[{0}]", string.Join(",", result));
        //}
    }
}