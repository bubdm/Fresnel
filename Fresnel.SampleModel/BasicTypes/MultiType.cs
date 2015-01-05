using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.SampleModel.BasicTypes
{

    public class MultiType 
    {

        public Guid ID { get; set; }

        public virtual bool A_Boolean { get; set; }

        public virtual char A_Char { get; set; }

        public virtual string A_String { get; set; }

        public virtual int An_Int { get; set; }

        public virtual double A_Double { get; set; }

        public virtual float A_Float { get; set; }

    }
}
