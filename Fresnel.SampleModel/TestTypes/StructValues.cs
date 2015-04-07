using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    /// <summary>
    /// A set of Struct properties
    /// </summary>
    public class StructValues
    {
        /// <summary>
        ///
        /// </summary>
        public StructValues()
        {
            this.ArrayOfNumbers = new int[] { 1, 2, 3, 5, 7, 11, 13 };

            this.ArrayOfColours = new Color[] { Colors.Red, Colors.Yellow, Colors.Pink, Colors.Green, Colors.Purple, Colors.Orange, Colors.Blue };
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        [Key]
        public Guid ID { get; set; }

        //        /// <summary>
        //        /// This property will not be visible, because it isn't a Value type
        //        /// </summary>
        //        public Font Font { get; set; }

        /// <summary>
        /// This will show a PropertyGrid with X/Y values.
        /// The background colour should change when the control gets focus
        /// </summary>
        public Point Point { get; set; }

        //        private Rectangle _Rectangle;
        //
        //        /// <summary>
        //        /// This will show a PropertyGrid with X/Y values.
        //        /// The Point value will change too, to show that associated properties are updated
        //        /// </summary>
        //        public Rectangle Rectangle
        //        {
        //            get { return _Rectangle; }
        //            set
        //            {
        //                _Rectangle = value;
        //                this.Point = new Point(this.Point.X + 1, this.Point.Y + 1);
        //            }
        //        }

        /// <summary>
        /// This will show a PropertyGrid with a colour selector
        /// </summary>
        public Color Colour { get; set; }

        /// <summary>
        /// This will show a PropertyGrid, but the value cannot be changed
        /// </summary>
        public Color ReadOnlyColour
        {
            get { return this.Colour; }
        }

        /// <summary>
        ///
        /// </summary>
        public int[] ArrayOfNumbers { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Color[] ArrayOfColours { get; set; }
    }
}