namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Configuration for a Collection Property
    /// </summary>
    public class CollectionPropertyConfiguration : BaseConfiguration
    {
        private bool _IsAggregateRelationship;
        private bool _IsCompositeRelationship;

        public CollectionPropertyConfiguration()
            : base()
        {
            this.IsCompositeRelationship = false;
            this.IsAggregateRelationship = true;
            //this.CanExpandContents = true;

            this.CanAdd = true;
            this.CanRemove = true;
            //this.HasUniqueItems = true;
            //this.InlineColumnNames = new string[] { };
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
                    //this.CanCreate = true;
                    //this.CanExpandContents = true;
                    this.CanAdd = false;
                    this.CanRemove = true;
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
                    //this.CanCreate = false;
                    //this.CanExpandContents = false;
                    this.CanAdd = true;
                    this.CanRemove = true;
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

        ///// <summary>
        ///// Determines if a Domain Object can be added to the Collection multiple times
        ///// </summary>
        //public bool HasUniqueItems { get; set; }

        ///// <summary>
        ///// A list of Property names that are shown when displayed in-line.
        ///// Used in conjunction with IsShownInline.
        ///// </summary>
        //public string[] InlineColumnNames { get; set; }
    }
}