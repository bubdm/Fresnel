using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    public class MultiTypeFactory : IFactory<MultiType>
    {

        public MultiType Create()
        {
            var newObj = new MultiType();
            newObj.ID = Guid.NewGuid();
            newObj.A_DateTime = DateTime.Now;
            newObj.A_DateTimeOffset = DateTimeOffset.Now;

            return newObj;
        }
    }
}