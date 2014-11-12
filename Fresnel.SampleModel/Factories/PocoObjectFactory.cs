using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;
using Envivo.Sample.Model.Objects;

namespace Envivo.Sample.Model.Factories
{
    public class PocoObjectFactory : IFactory<PocoObject>
    {

        public PocoObject Create()
        {
            return new PocoObject();
        }

        public PocoObject Create(PocoObject parent)
        {
            var result = new PocoObject();

            result.NormalText = "Owned by " + parent.ID.ToString();

            return result;
        }

    }
}
