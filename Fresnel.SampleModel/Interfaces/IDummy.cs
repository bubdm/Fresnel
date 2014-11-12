using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model
{
    public interface IDummy
    {

        /// <summary>
        /// The name of this Product
        /// </summary>
        string Name { get; set; }

    }
}
