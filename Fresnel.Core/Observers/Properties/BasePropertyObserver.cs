using Envivo.Fresnel.Introspection.Templates;
using System;

namespace Envivo.Fresnel.Core.Observers
{

    /// <summary>
    /// An Observer for a Property belonging to a Object
    /// </summary>

    public abstract class BasePropertyObserver : BaseMemberObserver
    {
        //private object _Value;
        //private BaseObjectObserver _oInnerObserver = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentObject">The ObjectObserver that owns this Property</param>
        /// <param name="propertyTemplate">The PropertyTemplate that reflects the Property</param>

        internal BasePropertyObserver(ObjectObserver oParent, PropertyTemplate tSourceProperty)
            : base(oParent, tSourceProperty)
        {
        }

        public new PropertyTemplate Template
        {
            get { return (PropertyTemplate)base.Template; }
        }

        public DateTime LastUpdatedAtUtc { get; internal set; }

        ///// <summary>
        ///// Sets the value of the backing field directly
        ///// </summary>
        ///// <param name="value"></param>
        //internal void SetField(object value)
        //{
        //    // We're replacing the value, so disassociate the existing value from this property:
        //    //this.InnerObserver.DisassociateFrom(this);
        //    if (_oInnerObserver != null)
        //    {
        //        _oInnerObserver.DisassociateFrom(this);
        //    }

        //    this.PropertyTemplate.SetField(this.RealObject, value);

        //    this.OuterObject.ChangeTracker.HasChanges = true;

        //    // Make the object aware that it is associated with this property:
        //    this.InnerObserver.AssociateWith(this);
        //}

        ///// <summary>
        ///// The Observer that proxies this Property's *value*
        ///// </summary>
        ///// <value>The Observer that proxies this Property's value</value>
        ///// <remarks>
        ///// We cannot know what the property's value is at any point in time (it may have been updated by some business logic).
        ///// For that reason, we have to ask for the appropriate Observer.
        ///// </remarks>
        //public override BaseObjectObserver InnerObserver
        //{
        //    get
        //    {
        //        if (this.Permissions.Read.Check().Passed)
        //        {
        //            BaseObjectObserver oParent;
        //            object value = this.Value;

        //            // NB: It's CRITICAL that the value is re-fetched from the property (in case a lazy-load proxy has replaced it):
        //            var realObjectType = (value != null) ? this.Value.GetRealType() : this.MemberType;

        //            oParent = this.Session.GetObserver(value, realObjectType);

        //            if (oParent.IsNull == false)
        //            {
        //                // Make the object aware that it is associated with this property:
        //                oParent.AssociateWith(this);
        //            }

        //            _oInnerObserver = oParent;
        //            return oParent;
        //        }
        //        else
        //        {
        //            _oInnerObserver = null;
        //            return this.NullObserver;
        //        }
        //    }
        //    //TODO: Change InnerObserver to Read-only
        //    set { }
        //}

        ///// <summary>
        ///// Returns the value of the property, by-passing the lazy-load checks
        ///// </summary>
        //public virtual object GetValueByForce()
        //{
        //    return this.PropertyTemplate.GetValue(this.RealObject);
        //}

        /// <summary>
        /// Determines if the value is Nothing
        /// </summary>
        /// <value>True = The Property's value is Nothing</value>


        //public override bool IsNull
        //{
        //    get
        //    {
        //        if (this.IsReflectionEnabled == false)
        //            return (_Value == null);

        //        // Optimisation: This prevents an InnerObserver being created for no reason:
        //        try
        //        {
        //            return this.PropertyTemplate.GetProperty(this.RealObject) == null;
        //        }
        //        catch (Exception ex)
        //        {
        //            // If the Domain Property throws an exception, the framework expects the ErrorMessage to be set:
        //            this.ErrorMessage = this.CreateDescriptiveErrorMessage(ex.Message);
        //            return true;
        //        }
        //    }
        //    set { base.IsNull = value; }
        //}

        //private string CreateDescriptiveErrorMessage(string errorMessage)
        //{
        //    var message = string.Format("Unable to read '{0}'", this.FriendlyName);
        //    return string.Concat(message, Environment.NewLine, errorMessage);
        //}

        //public bool IsReferenceType
        //{
        //    get { return this.PropertyTemplate.IsNonReference == false; }
        //}

        ///// <summary>
        ///// Determines if the value of the Property is a Non-Reference value
        ///// </summary>
        ///// <value>True = The value of the Property Is a Non-Reference value</value>
        //public bool IsNonReference
        //{
        //    get { return this.PropertyTemplate.IsNonReference; }
        //}

        ///// <summary>
        ///// Determines if the value of the Property is a Domain Object
        ///// </summary>
        ///// <value>True = The value of the Property is a Domain Object</value>
        //public bool IsObject
        //{
        //    get { return this.PropertyTemplate.IsDomainObject; }
        //}

        ///// <summary>
        ///// Determines if the value of the Property is an ValueObject
        ///// </summary>
        //public bool IsValueObject
        //{
        //    get { return this.PropertyTemplate.IsValueObject; }
        //}

        ///// <summary>
        ///// Determines if the value of the Property is a collection/list
        ///// </summary>
        ///// <value>True = The value of the Property is a collection/list</value>
        //public bool IsCollection
        //{
        //    get { return this.PropertyTemplate.IsCollection; }
        //}

        //public override string ToString()
        //{
        //    if (this.OuterObject == null)
        //    {
        //        return base.ToString();
        //    }
        //    else
        //    {
        //        return string.Concat(this.OuterObject.ToString(), ".", this.FriendlyName);
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
        //    var oInnerObj = this.InnerObserver;
        //    var text = string.Empty;

        //    if (oInnerObj.IsNull == false)
        //    {
        //        if (oInnerObj.IsNonReference)
        //        {
        //            // If we're dealing with a primitve, we want to make sure it's formatted correctly.
        //            // The ValueObserver can only determine the formatting through the Property's PropertyAttribute:
        //            oInnerObj.InnerNonReference.LoadAttributes(this.PropertyTemplate.Attributes);
        //            text = oInnerObj.ToString(showStatus, showName, showSummary);
        //        }
        //        else if (oInnerObj.IsObject || oInnerObj.IsValueObject)
        //        {
        //            text = oInnerObj.ToString(showStatus, showName, showSummary);
        //        }
        //    }

        //    if (text.IsEmpty())
        //    {
        //        if (showName)
        //        {
        //            text = this.FriendlyName;
        //        }
        //    }
        //    return text;
        //}

        ///// <summary>
        ///// Returns a token (Memento) for the Observer
        ///// </summary>
        //
        //
        //public abstract Tokens.PropertyToken GetToken();

        public override void Dispose()
        {
            //_Value = null;
            //_oInnerObserver = null;
            base.Dispose();
        }

    }
}
