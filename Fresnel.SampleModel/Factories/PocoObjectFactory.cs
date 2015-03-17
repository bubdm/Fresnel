using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;
using System;

namespace Envivo.Sample.Model.Factories
{
    public class PocoObjectFactory : IFactory<PocoObject>
    {
        private IPersistenceService _PersistenceService;

        public PocoObjectFactory(IPersistenceService persistenceService)
        {
            _PersistenceService = persistenceService;
        }

        public PocoObject Create()
        {
            var newObject = _PersistenceService.CreateObject<PocoObject>();
            newObject.ID = Guid.NewGuid();
            newObject.NormalText = "This was created using PocoObjectFactory.Create()";

            return newObject;
        }

        public PocoObject Create(PocoObject parent)
        {
            var newObject = this.Create();
            newObject.ID = Guid.NewGuid();
            newObject.NormalText = "This was created using PocoObjectFactory.Create(parent)";

            return newObject;
        }
    }
}