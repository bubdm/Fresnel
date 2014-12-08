using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;
using System.Windows;
using System.Windows.Media;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    /// <summary>
    /// A set of Struct properties
    /// </summary>
    public class StructValues
    {

        public StructValues()
        {
            this.ArrayOfNumbers = new int[] { 1, 2, 3, 5, 7, 11, 13 };

            this.ArrayOfColours = new Color[] { Colors.Red, Colors.Yellow, Colors.Pink, Colors.Green, Colors.Purple, Colors.Orange, Colors.Blue };
        }

        public virtual Guid ID { get; set; }

        //        /// <summary>
        //        /// This property will not be visible, because it isn't a Value type
        //        /// </summary>
        //        public Font Font { get; set; }

        /// <summary>
        /// This will show a PropertyGrid with X/Y values.
        /// The background colour should change when the control gets focus
        /// </summary>
        public virtual Point Point { get; set; }

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
        public virtual Color Colour { get; set; }

        /// <summary>
        /// This will show a PropertyGrid, but the value cannot be changed
        /// </summary>
        public virtual Color ReadOnlyColour
        {
            get { return this.Colour; }
        }

        public virtual int[] ArrayOfNumbers { get; set; }

        public virtual Color[] ArrayOfColours { get; set; }

    }
}
