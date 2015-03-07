using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for a Domain Object or List Property
    /// </summary>

    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Enum, AllowMultiple = true)]
    public class ObjectPropertyBaseConfiguration : PropertyConfiguration
    {
        private bool _IsShownInline;

        public ObjectPropertyBaseConfiguration()
            : base()
        {
            this.CanCreate = true;
            this.IsLazyLoaded = true;
            this.CanModifyContents = true;
            this.CanExpandContents = true;
            this.IsShownInline = false;
        }

        /// <summary>
        /// Determines if a new object can be created for this property in the UI.
        /// Use this to prevent objects being created in the wrong operational context.
        /// </summary>
        /// <value></value>

        public bool CanCreate { get; set; }

        /// <summary>
        /// Determines whether the property is loaded only when explicitly requested by the user. Defaults to TRUE.
        /// Note that this only takes effect on properties that contain Objects.
        /// If set to FALSE, the object is Eager loaded.
        /// </summary>
        /// <value></value>

        public bool IsLazyLoaded { get; set; }

        /// <summary>
        /// Determines whether the user is allowed to modify the property's contents.
        /// A value of FALSE means that the properties contents must be edited from another Object Root.
        /// This supports the "Aggregate Root" constraint in DDD.
        /// </summary>
        public bool CanModifyContents { get; set; }

        /// <summary>
        /// Determines if the user can expand nested objects/collections.
        /// This is useful for limiting the amount of information visible, and aiding usability.
        /// Property relationships will affect this value.
        /// </summary>
        public bool CanExpandContents { get; set; }

        /// <summary>
        /// The type of Query Specification that is used to pre-populate a context driven Search Query
        /// </summary>
        public Type SearchQueryFilter { get; set; }

        ///// <summary>
        ///// Only allows the specified typesto be used with the property.
        ///// NB. The types *must* be a subclass of the property's type.
        ///// </summary>
        //public Type OverrideType { get; set; }

        /// <summary>
        /// Determines if the property's contents are shown inline. Useful for showing summary info, without having to expand the object.
        /// </summary>
        public bool IsShownInline
        {
            get { return _IsShownInline; }
            set
            {
                _IsShownInline = value;
                if (_IsShownInline)
                {
                    // Make the property eager load:
                    this.IsLazyLoaded = false;
                }
            }
        }
    }
}