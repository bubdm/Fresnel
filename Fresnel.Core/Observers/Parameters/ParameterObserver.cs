using Envivo.Fresnel.Introspection.Templates;
using Newtonsoft.Json;

namespace Envivo.Fresnel.Core.Observers
{
    /// <summary>
    /// An Observer for a Parameter belonging to a Member
    /// </summary>

    public class ParameterObserver : BaseMemberObserver
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="oProperty">The MethodObserver that owns this Parameter</param>
        /// <param name="tParameter">The ParameterTemplate that reflects the parameter</param>
        public ParameterObserver(MethodObserver oParentMethod, ParameterTemplate tParameter)
            : base(oParentMethod.OuterObject, tParameter)
        {
            this.OuterMethod = oParentMethod;
        }

        [JsonIgnore]
        public new ParameterTemplate Template
        {
            get { return (ParameterTemplate)base.Template; }
        }

        [JsonIgnore]
        internal override string DebugID
        {
            get
            {
                var tClass = this.OuterMethod.OuterObject.Template;
                var tMethod = this.OuterMethod.Template;

                return string.Concat("[", this.OuterMethod.OuterObject.ID, " ",
                                          tClass.FullName, ".+",
                                          tMethod.Name, "(", this.Template.Name, ")]");
            }
        }

        //public override bool IsNull
        //{
        //    get { return (this.Value == null); }
        //    set { base.IsNull = value; }
        //}

        /// <summary>
        /// The Value of the Parameter
        /// </summary>
        /// <remarks>Unlike the BasePropertyObserver, the ParameterReflection doesn't give us access it's values</remarks>

        public object Value { get; set; }

        /// <summary>
        /// The MethodObserver that owns this Parameter
        /// </summary>
        [JsonIgnore]
        public MethodObserver OuterMethod { get; private set; }

        /////// <summary>
        /////// The Observer that proxies this Parameter's *value*
        /////// </summary>

        ////public override BaseObjectObserver InnerObserver
        ////{
        ////    get
        ////    {
        ////        var oParent = this.Session.GetObserver(this.Value, this.MemberType);

        ////        // Make the object aware that it is associated with this parameter:
        ////        if (oParent.OuterParameters.DoesNotContain(this))
        ////        {
        ////            oParent.OuterParameters.Add(this);
        ////        }

        ////        return oParent;
        ////    }
        ////    // Instead of storing the Observer, we'll store the value to it (the Observer is determined dynamically):
        ////    set { this.Value = value.RealObject; }
        ////}

        /// <summary>
        /// Determines if the value of the Parameter is a Non-Reference value
        /// </summary>
        /// <value>True = The value of the Parameter is a Non-Reference value</value>
        public bool IsNonReference
        {
            get { return (this.InnerObserver is NonReferenceObserver); }
        }

        /// <summary>
        /// Determines if the value of the Parameter is a Domain Object
        /// </summary>
        /// <value>True = The value of the Parameter is a Domain Object</value>
        public bool IsObject
        {
            get { return (this.InnerObserver is ObjectObserver) && (this.IsCollection == false); }
        }

        /// <summary>
        /// Determines if the value of the Parameter is a List
        /// </summary>
        /// <value>True = The value of the Parameter is a List</value>
        public bool IsCollection
        {
            get { return (this.InnerObserver is CollectionObserver); }
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="showStatus"></param>
        ///// <param name="showName"></param>
        ///// <param name="showSummary"></param>

        ///// <remarks>This method must be as optimal as possible, as it will be called many times in rapid succession</remarks>
        //public override string ToString(bool showStatus, bool showName, bool showSummary)
        //{
        //    return this.InnerObserver.ToString(showStatus, showName, showSummary);
        //}

        ///// <summary>
        ///// Returns a token (Memento) for the Observer
        ///// </summary>
        //
        //
        //public ParameterToken GetToken()
        //{
        //    return new ParameterToken(this);
        //}

        public override void Dispose()
        {
            this.OuterMethod = null;
            //this.MemberType = null;
            this.Value = null;
            base.Dispose();
        }
    }
}