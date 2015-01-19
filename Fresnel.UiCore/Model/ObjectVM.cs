using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Model
{
    public class ObjectVM : BaseViewModel
    {
        public Guid ID { get; set; }

        public string Type { get; set; }

        public IEnumerable<PropertyVM> Properties { get; set; }

        public IEnumerable<MethodVM> Methods { get; set; }
    }
}