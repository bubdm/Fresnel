using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    public class MultiType
    {
        private TextValues _An_Object;
        private ICollection<BooleanValues> _A_Collection = new List<BooleanValues>();

        public MultiType()
        {
        }

        public MultiType(TextValues anObject)
            : this()
        {
            this.An_Object = anObject;
        }

        [Key]
        public Guid ID { get; set; }

        public bool A_Boolean { get; set; }

        public char A_Char { get; set; }

        public string A_String { get; set; }

        public int An_Int { get; set; }

        public double A_Double { get; set; }

        public float A_Float { get; set; }

        public DateTime A_DateTime { get; set; }

        public DateTimeOffset A_DateTimeOffset { get; set; }

        public EnumValues.IndividualOptions An_Enum { get; set; }

        public CombinationOptions A_Bitwise_Enum { get; set; }

        public virtual TextValues An_Object
        {
            get { return _An_Object; }
            set { _An_Object = value; }
        }

        public virtual ICollection<BooleanValues> A_Collection
        {
            get { return _A_Collection; }
            set { _A_Collection = value; }
        }
    }
}