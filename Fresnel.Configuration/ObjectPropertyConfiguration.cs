using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for a Domain Object Property
    /// </summary>

    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Enum, AllowMultiple = true)]
    public class ObjectPropertyConfiguration : ObjectPropertyBaseConfiguration
    {
        private SingleRelationship _Relationship;

        public ObjectPropertyConfiguration()
            : base()
        {
            this.Relationship = SingleRelationship.HasA;
        }

        /// <summary>
        /// Determines the relationship between the parent Object and this property's Object.
        /// The default relationship is "HasA".
        /// </summary>
        /// <value></value>

        public SingleRelationship Relationship
        {
            get { return _Relationship; }
            set
            {
                _Relationship = value;

                switch (value)
                {
                    case SingleRelationship.OwnedBy:
                    case SingleRelationship.HasA:
                        this.CanCreate = false;
                        this.CanExpandContents = false;
                        break;

                    case SingleRelationship.OwnsA:
                        this.CanExpandContents = true;
                        this.CanCreate = true;
                        break;
                }
            }
        }

        /// <summary>
        /// The type of Query Specification that is used to populate a selection list
        /// </summary>
        public Type LookupListFilter { get; set; }
    }
}