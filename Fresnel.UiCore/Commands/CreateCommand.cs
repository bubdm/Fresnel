﻿using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CreateCommand : ICommand
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private IPersistenceService _PersistenceService;
        private CreateObjectCommand _CreateObjectCommand;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private IClock _Clock;

        public CreateCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            IPersistenceService persistenceService,
            CreateObjectCommand createObjectCommand,
            AbstractObjectVmBuilder objectVMBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _PersistenceService = persistenceService;
            _CreateObjectCommand = createObjectCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _Clock = clock;
        }

        public CreateCommandResponse Invoke(string fullyQualifiedName)
        {
            try
            {
                var tClass = _TemplateCache.GetTemplate(fullyQualifiedName);
                if (tClass == null)
                    return null;

                var newObject = _PersistenceService.CreateObject(tClass.RealType);

                var oObject = newObject != null ?
                                _ObserverCache.GetObserver(newObject, tClass.RealType) :
                                _CreateObjectCommand.Invoke(tClass.RealType, null);

                var vm = _ObjectVMBuilder.BuildFor(oObject);

                return new CreateCommandResponse()
                {
                    Passed = true,
                    NewObject = vm
                };
            }
            catch (Exception ex)
            {
                var errorVM = new MessageVM()
                {
                    IsError = true,
                    OccurredAt = _Clock.Now,
                    Text = ex.Message,
                    Detail = ex.ToString(),
                };

                return new CreateCommandResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}