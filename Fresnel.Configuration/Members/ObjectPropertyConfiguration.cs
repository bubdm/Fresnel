using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Domain Object Property
    /// </summary>
    public class ObjectPropertyConfiguration : BaseConfiguration
    {
        private bool _IsAggregateRelationship;
        private bool _IsCompositeRelationship;
        private bool _IsParentRelationship;

        public ObjectPropertyConfiguration()
            : base()
        {
            this.IsAggregateRelationship = true;
        }

        /// <summary>
        /// Determines if the property has an Aggregate relationship with the contents
        /// </summary>
        /// <value></value>
        public bool IsAggregateRelationship
        {
            get { return _IsAggregateRelationship; }
            set
            {
                _IsAggregateRelationship = value;
                if (value)
                {
                    _IsCompositeRelationship = false;
                    _IsParentRelationship = false;
                    //this.CanCreate = true;
                    //this.CanExpandContents = true;
                }
            }
        }

        /// <summary>
        /// Determines if the property has a Composite Aggregate relationship with the contents
        /// </summary>
        /// <value></value>
        public bool IsCompositeRelationship
        {
            get { return _IsCompositeRelationship; }
            set
            {
                _IsCompositeRelationship = value;
                if (value)
                {
                    _IsAggregateRelationship = false;
                    _IsParentRelationship = false;
                    //this.CanCreate = false;
                    //this.CanExpandContents = true;
                }
            }
        }

        /// <summary>
        /// Determines if the class is the parent of the property contents
        /// </summary>
        /// <value></value>
        public bool IsParentRelationship
        {
            get { return _IsParentRelationship; }
            set
            {
                _IsParentRelationship = value;
                if (value)
                {
                    _IsAggregateRelationship = false;
                    _IsCompositeRelationship = false;
                    //this.CanCreate = false;
                    //this.CanExpandContents = false;
                }
            }
        }

        ///// <summary>
        ///// The type of Query Specification that is used to populate a selection list
        ///// </summary>
        //public Type LookupListFilter { get; set; }
    }
}