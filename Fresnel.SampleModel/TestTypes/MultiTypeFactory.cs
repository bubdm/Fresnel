using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.TestTypes
{
    public class MultiTypeFactory : IFactory<MultiType>
    {
        private IPersistenceService _PersistenceService;

        public MultiTypeFactory(IPersistenceService persistenceService)
        {
            _PersistenceService = persistenceService;
        }

        public MultiType Create()
        {
            var newObj = _PersistenceService.CreateObject<MultiType>();
            if (newObj == null)
                return null;

            newObj.ID = Guid.NewGuid();
            newObj.A_DateTime = DateTime.Now;
            newObj.A_DateTimeOffset = DateTimeOffset.Now;

            return newObj;
        }
    }
}