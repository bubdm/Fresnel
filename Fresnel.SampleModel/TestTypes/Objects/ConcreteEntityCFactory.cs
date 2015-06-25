using Envivo.Fresnel.DomainTypes.Interfaces;
using System;

namespace Envivo.Fresnel.SampleModel.TestTypes.Objects
{
    public class ConcreteEntityCFactory : IFactory<ConcreteEntityC>
    {

        public ConcreteEntityC Create()
        {
            var newObj = new ConcreteEntityC();
            newObj.ID = Guid.NewGuid();
            newObj.A_StringValue = "This was created by the default Factory method";

            return newObj;
        }

        public ConcreteEntityC Create(string stringValue)
        {
            var newObj = new ConcreteEntityC();
            newObj.ID = Guid.NewGuid();
            newObj.A_StringValue = "This value was provided via the Factory method: " + stringValue;

            return newObj;
        }
    }
}