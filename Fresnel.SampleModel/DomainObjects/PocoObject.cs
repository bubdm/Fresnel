using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Sample.Model.BasicTypes;
using Envivo.DomainTypes;using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.Objects
{
    /// <summary>
    /// This class is not inherited, and doesn't use custom attributes.
    /// It is configured externally by the PocoObjectConfiguration class in the launcher application.
    /// </summary>
    public class PocoObject
    {

        private ICollection<PocoObject> _ChildObjects = new Collection<PocoObject>();
        private DateTime _NormalDate = DateTime.UtcNow;
        
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var that = obj as PocoObject;
            if (that == null)
                return false;

            return this.ID.Equals(that.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
        
        public virtual Guid ID { get; set; }

        public virtual long Version { get; set; }

        public virtual int RecordId { get; protected set; }

        /// <summary>
        /// This is a normal Boolean
        /// </summary>
        [Property(Category = "Sort 1")]
        public virtual bool NormalBoolean { get; set; }


        /// <summary>
        /// This is a Rich Text field, and should appear as a large editor
        /// </summary>
        public virtual string FormattedText { get; set; }

        /// <summary>
        /// This is an unformatted Date.
        /// Clicking the down-arrow will reveal the DatePicker dialog.
        /// </summary>
        public virtual DateTime NormalDate
        {
            get { return _NormalDate; }
            set { _NormalDate = value; }
        }

        /// <summary>
        /// This enum should be shown as a drop-down list.
        /// </summary>
        [Property(Category = "Sort 2")]
        public virtual EnumValues.IndividualOptions EnumValue { get; set; }

        /// <summary>
        /// This enum should be shown as a check-list.
        /// </summary>
        public virtual CombinationOptions EnumSwitches { get; set; }

        /// <summary>
        /// This is a normal Text
        /// </summary>
        public virtual string NormalText { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// It should also be possible to 'Cut' this value using the clipboard commands.
        /// </summary>
        public virtual int? NullableInt { get; set; }

        /// <summary>
        /// This will show a PropertyGrid with X/Y values.
        /// The background colour should change when the control gets focus
        /// </summary>
        public virtual System.Drawing.Point Point { get; set; }

        public virtual ICollection<PocoObject>ChildObjects
        {
            get { return _ChildObjects; }
            protected set { _ChildObjects = value; }
        }

        /// <summary>
        /// This wil add some objects to the "Child Objects" property.
        /// Actions can only be invoked if there are no unsaved objects on the Workbench.
        /// </summary>
        public virtual void AddSomeChildObjects()
        {
            this.ChildObjects.Add(new PocoObject());
            this.ChildObjects.Add(new PocoObject());
            this.ChildObjects.Add(new PocoObject());
        }

        /// <summary>
        /// Creates a POCO object with lots of Child objects
        /// </summary>
        /// <returns></returns>
        public static PocoObject CreateLargeObject()
        {
            var result = new PocoObject();
            for (var i = 1; i <= 500; i++)
            {
                result.ChildObjects.Add(new PocoObject() { RecordId = i });
            }
            return result;
        }


        /// <summary>
        /// This static method will appear on the Class item in the Library.
        /// All Objects that are created will be automatically persisted.
        /// </summary>
        /// <returns></returns>
        public static PocoObject CreateWithSomeChildObjects()
        {
            var result = new PocoObject();
            result.FormattedText = "I was created from a Static method";

            result.AddSomeChildObjects();

            return result;
        }

    }
}
