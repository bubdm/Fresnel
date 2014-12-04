using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Envivo.Fresnel.Core.Observers
{

    /// <summary>
    /// An Observer for a Non-Reference value (e.g. primitives, structs, and non-reference types)
    /// </summary>
    public class NonReferenceObserver : BaseObjectObserver
    {


        public NonReferenceObserver(object nonRefValue, Type nonRefType, NonReferenceTemplate tNonRef)
            : base(nonRefValue, nonRefType, tNonRef)
        {
            //LoadAttributes(this.Template.Attributes);
        }

        [JsonIgnore]
        public new NonReferenceTemplate Template
        {
            get { return (NonReferenceTemplate)base.Template; }
        }

        /// <summary>
        /// The underlying Template used to create this Observer
        /// </summary>
        
        //public  NonReferenceTemplate NonReferenceTemplate
        //{
        //    get { return (NonReferenceTemplate)base.Template; }
        //}

        //internal void LoadAttributes(AttributesCollection sourceAttributes)
        //{
        //    _NumberAttribute = sourceAttributes.Get<NumberAttribute>();
        //    _BooleanAttribute = sourceAttributes.Get<BooleanAttribute>();
        //    _DateTimeAttribute = sourceAttributes.Get<DateTimeAttribute>();
        //    _StringAttribute = sourceAttributes.Get<TextAttribute>();
        //}

        ///// <summary>
        ///// The Member Observer that owns this object
        ///// </summary>
        ///// <value>The Member Observer that owns this object</value>
        ///// <remarks>Simple objects are mostly non-reference types, and therefore can only have ONE owner.</remarks>
        //internal BaseObserver OuterMember
        //{
        //    get
        //    {
        //        // First we'll try the OuterProperties:
        //        foreach (var oProperty in this.OuterProperties)
        //        {
        //            return oProperty;
        //        }
        //        // We didn't find anything, so let's try the OuterParameters:
        //        foreach (var oParameter in this.OuterParameters)
        //        {
        //            return oParameter;
        //        }
        //        // Nothing was found:
        //        return null;
        //    }
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
        //        var value = this.GetFormattedValue();
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
        ///// <returns>The Formatted value as a String</returns>
        
        //internal string GetFormattedValue()
        //{
        //    if (this.RealObject == null)
        //        return string.Empty;

        //    // Use the local regional settings to format the value
        //    switch (this.Template.KindOf)
        //    {
        //        case TypeKind.Text:
        //            return this.RealObject.ToString();

        //        case TypeKind.Integer:
        //            return this.GetIntegerString();

        //        case TypeKind.Floating:
        //            return this.GetFloatingString();

        //        case TypeKind.Boolean:
        //            return this.ToFormattedBoolean();

        //        case TypeKind.Time:
        //            return this.GetDateTimeString();

        //        default:
        //            return this.GetString();

        //    }
        //}

        //private string GetIntegerString()
        //{
        //    if (_NumberAttribute.IsCurrency)
        //    {
        //        return GetCurrencyString();
        //    }
        //    else
        //    {
        //        return this.RealObject.ToString();
        //    }
        //}

        //private string GetFloatingString()
        //{
        //    if (object.Equals(this.RealObject, double.NaN))
        //    {
        //        return "Not a number";
        //    }

        //    if (_NumberAttribute.IsCurrency)
        //    {
        //        return GetCurrencyString();
        //    }
        //    else
        //    {
        //        var value = this.RealObject.ToString();
        //        return value.ToString(System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
        //    }
        //}

        //private string GetCurrencyString()
        //{
        //    return Convert.ToDecimal(this.RealObject).ToString("c");
        //}

        //private string ToFormattedBoolean()
        //{
        //    var value = (bool)this.RealObject;
        //    if (value == true)
        //    {
        //        return _BooleanAttribute.TrueValue;
        //    }
        //    else
        //    {
        //        return _BooleanAttribute.FalseValue;
        //    }
        //}

        //private string GetDateTimeString()
        //{
        //    return ((DateTime)this.RealObject).ToString(_DateTimeAttribute.CustomFormat);
        //}

        //private string GetString()
        //{
        //    if (_StringAttribute.IsPassword)
        //    {
        //        return new string('*', this.RealObject.ToString().Length);
        //    }
        //    else
        //    {
        //        return this.RealObject.ToString();
        //    }
        //}

        public override void Dispose()
        {
            //_BooleanAttribute = null;
            //_DateTimeAttribute = null;
            //_NumberAttribute = null;
            //_StringAttribute = null;
            base.Dispose();
        }

    }

}
