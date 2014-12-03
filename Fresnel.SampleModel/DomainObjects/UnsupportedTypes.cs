using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.SampleModel.BasicTypes;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// Exposes known classes that are not fully supported
    /// </summary>
    public class UnsupportedTypes
    {

        private Dictionary<string, PocoObject> _Map = new Dictionary<string, PocoObject>();
        
        public virtual Guid ID { get; set; }

        public virtual Dictionary<string, PocoObject> Map
        {
            get { return _Map; }
            protected set { _Map = value; }
        }

    }
}
