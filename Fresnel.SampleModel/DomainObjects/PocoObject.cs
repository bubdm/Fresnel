using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.BasicTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// This class is not inherited, and doesn't use custom attributes.
    /// It is configured externally by the PocoObjectConfiguration class in the launcher application.
    /// </summary>
    public class PocoObject
    {

        private ICollection<PocoObject> _ChildObjects = new Collection<PocoObject>();
        private DateTime _NormalDate = DateTime.UtcNow;

        public PocoObject()
        {
        }

        public override string ToString()
        {
            return string.Concat(typeof(PocoObject).Name, "/", this.RecordId);
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int RecordId { get; protected set; }

        /// <summary>
        /// This is a normal Boolean
        /// </summary>
        [Display(GroupName = "Sort 1")]
        public bool NormalBoolean { get; set; }

        /// <summary>
        /// This is a Rich Text field, and should appear as a large editor
        /// </summary>
        public string FormattedText { get; set; }

        /// <summary>
        /// This is an unformatted Date.
        /// Clicking the down-arrow will reveal the DatePicker dialog.
        /// </summary>
        public DateTime NormalDate
        {
            get { return _NormalDate; }
            set { _NormalDate = value; }
        }

        /// <summary>
        /// This enum should be shown as a drop-down list.
        /// </summary>
        [Display(GroupName = "Sort 2")]
        public EnumValues.IndividualOptions EnumValue { get; set; }

        /// <summary>
        /// This enum should be shown as a check-list.
        /// </summary>
        public CombinationOptions EnumSwitches { get; set; }

        /// <summary>
        /// This is a normal Text
        /// </summary>
        public string NormalText { get; set; }

        /// <summary>
        /// It should be possible to clear this value.
        /// It should also be possible to 'Cut' this value using the clipboard commands.
        /// </summary>
        public int? NullableInt { get; set; }

        /// <summary>
        /// This will show a PropertyGrid with X/Y values.
        /// The background colour should change when the control gets focus
        /// </summary>
        public Point Point { get; set; }

        /// <summary>
        ///
        /// </summary>
        public virtual ICollection<PocoObject> ChildObjects
        {
            get { return _ChildObjects; }
            protected set { _ChildObjects = value; }
        }

        /// <summary>
        /// This wil method some objects to the "Child Objects" property.
        /// Actions can only be invoked if there are no unsaved objects on the Workbench.
        /// </summary>
        public void AddSomeChildObjects()
        {
            var stamp = Environment.TickCount;
            this.ChildObjects.Add(new PocoObject() { ID = Guid.NewGuid(), RecordId = stamp });
            this.ChildObjects.Add(new PocoObject() { ID = Guid.NewGuid(), RecordId = stamp + 1 });
            this.ChildObjects.Add(new PocoObject() { ID = Guid.NewGuid(), RecordId = stamp + 2 });
        }

        /// <summary>
        /// Creates a POCO object with lots of Child objects
        /// </summary>
        /// <returns></returns>
        public static PocoObject CreateLargeObject()
        {
            var result = new PocoObject();

            var stamp = Environment.TickCount;
            for (var i = 1; i <= 500; i++)
            {
                result.ChildObjects.Add(new PocoObject() { RecordId = stamp + i });
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