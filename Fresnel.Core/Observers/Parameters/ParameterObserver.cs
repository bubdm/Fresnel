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
        /// <param name="oParentMethod">The MethodObserver that owns this Parameter</param>
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

        /// <summary>
        /// The Value of the Parameter
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The MethodObserver that owns this Parameter
        /// </summary>
        [JsonIgnore]
        public MethodObserver OuterMethod { get; private set; }

        /// <summary>
        /// Determines if the value of the Parameter is a Non-Reference value
        /// </summary>
        public bool IsNonReference
        {
            get { return (this.InnerObserver is NonReferenceObserver); }
        }

        /// <summary>
        /// Determines if the value of the Parameter is a Domain Object
        /// </summary>
        public bool IsObject
        {
            get { return (this.InnerObserver is ObjectObserver) && (this.IsCollection == false); }
        }

        /// <summary>
        /// Determines if the value of the Parameter is a List
        /// </summary>
        public bool IsCollection
        {
            get { return (this.InnerObserver is CollectionObserver); }
        }

        public override void Dispose()
        {
            this.OuterMethod = null;
            this.Value = null;
            base.Dispose();
        }
    }
}