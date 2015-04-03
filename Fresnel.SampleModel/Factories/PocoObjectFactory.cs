using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Objects;
using System;

namespace Envivo.SampleModel.Factories
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
            if (newObject == null)
                return null;

            newObject.ID = Guid.NewGuid();
            newObject.NormalText = "This was created using PocoObjectFactory.Create()";

            return newObject;
        }

        public PocoObject Create(PocoObject parent)
        {
            var newObject = this.Create();
            if (newObject == null)
                return null;

            newObject.ID = Guid.NewGuid();
            newObject.NormalText = "This was created using PocoObjectFactory.Create(parent)";

            return newObject;
        }
    }
}