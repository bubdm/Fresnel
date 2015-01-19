using System;
using System.Xml.Serialization;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for a Collection Property
    /// </summary>

    [Serializable()]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Enum, AllowMultiple = true)]
    public class CollectionPropertyAttribute : ObjectPropertyBaseAttribute
    {
        private ManyRelationship _Relationship;

        public CollectionPropertyAttribute()
            : base()
        {
            this.Relationship = ManyRelationship.HasMany;
            this.CanExpandContents = true;

            this.CanAdd = true;
            this.CanRemove = true;
            this.HasUniqueItems = true;
            this.InlineColumnNames = new string[] { };
        }

        /// <summary>
        /// Determines the relationship between the parent Object and the Domain Objects within the Collection.
        /// The default relationship is "HasMany".
        /// </summary>
        /// <value></value>

        [XmlAttribute()]
        public ManyRelationship Relationship
        {
            get { return _Relationship; }
            set
            {
                _Relationship = value;

                switch (value)
                {
                    case ManyRelationship.HasMany:
                        this.CanCreate = false;
                        this.CanExpandContents = false;
                        this.CanAdd = true;
                        this.CanRemove = true;
                        break;

                    case ManyRelationship.OwnsMany:
                        this.CanCreate = true;
                        this.CanExpandContents = true;
                        this.CanAdd = false;
                        this.CanRemove = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Determines if Domain Objects may be added to the Collection in the UI.
        /// Use this to prevent Domain Objects being added in the wrong operational context.
        /// </summary>
        /// <value></value>

        public bool CanAdd { get; set; }

        /// <summary>
        /// Determines if Domain Objects may be removed from the Collection in the UI.
        /// Use this to prevent Domain Objects being removed in the wrong operational context.
        /// </summary>
        /// <value></value>

        public bool CanRemove { get; set; }

        /// <summary>
        /// Determines if a Domain Object can be added to the Collection multiple times
        /// </summary>
        public bool HasUniqueItems { get; set; }

        /// <summary>
        /// A list of Property names that are shown when displayed in-line.
        /// Used in conjunction with IsShownInline.
        /// </summary>
        public string[] InlineColumnNames { get; set; }
    }
}