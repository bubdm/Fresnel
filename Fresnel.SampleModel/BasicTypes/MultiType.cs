using Envivo.Fresnel.Configuration;
using System;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{
    public class MultiType
    {
        public Guid ID { get; set; }

        public bool A_Boolean { get; set; }

        public char A_Char { get; set; }

        public string A_String { get; set; }

        public int An_Int { get; set; }

        public double A_Double { get; set; }

        public float A_Float { get; set; }

        public DateTime A_DateTime { get; set; }

        public DateTimeOffset A_DateTimeOffset { get; set; }
    }
}