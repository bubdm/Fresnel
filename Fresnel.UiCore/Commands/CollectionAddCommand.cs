using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CollectionAddCommand : ICommand
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private CreateObjectCommand _CreateObjectCommand;
        private AddToCollectionCommand _AddToCollectionCommand;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private ModificationsVmBuilder _ModificationsBuilder;
        private IClock _Clock;

        public CollectionAddCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            CreateObjectCommand createObjectCommand,
            AddToCollectionCommand addToCollectionCommand,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _CreateObjectCommand = createObjectCommand;
            _AddToCollectionCommand = addToCollectionCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public GenericResponse Invoke(CollectionAddNewRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oCollection = (CollectionObserver)_ObserverCache.GetObserverById(request.CollectionID);
                if (oCollection == null)
                    throw new UiCoreException("Cannot find collection with ID " + request.CollectionID);

                ObjectObserver oObject = null;
                var tClass = _TemplateCache.GetTemplate(request.ElementTypeName);
                if (tClass != null)
                {
                    oObject = (ObjectObserver)_CreateObjectCommand.Invoke(tClass.RealType, null);
                    if (oObject == null)
                        throw new UiCoreException("Cannot create object of type " + tClass.FriendlyName);
                }

                var oResult = _AddToCollectionCommand.Invoke(oCollection, oObject);

                // Other objects may have been affected by the action:
                _ObserverCache.ScanForChanges();

                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = string.Concat("Created and added new ", oObject.Template.FriendlyName)
                };
                return new GenericResponse()
                {
                    Passed = true,
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt),
                    Messages = new MessageVM[] { infoVM }
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

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }

        public GenericResponse Invoke(CollectionAddRequest request)
        {
            try
            {
                var messages = new List<MessageVM>();

                var startedAt = SequentialIdGenerator.Next;

                var oCollection = (CollectionObserver)_ObserverCache.GetObserverById(request.CollectionID);
                if (oCollection == null)
                    throw new UiCoreException("Cannot find collection with ID " + request.CollectionID);

                var oObjects = new List<ObjectObserver>();
                foreach (var elementID in request.ElementIDs)
                {
                    var oObject = (ObjectObserver)_ObserverCache.GetObserverById(elementID);
                    if (oObject == null)
                        throw new UiCoreException("Cannot find object with ID " + elementID);

                    oObjects.Add(oObject);
                    var oResult = _AddToCollectionCommand.Invoke(oCollection, oObject);

                    var infoVM = new MessageVM()
                    {
                        IsInfo = true,
                        OccurredAt = _Clock.Now,
                        Text = string.Concat("Added ", oObject.RealObject.ToString(), " to the collection")
                    };
                    messages.Add(infoVM);
                }

                // Other objects may have been affected by the action:
                _ObserverCache.ScanForChanges();

                if (oObjects.Count > 1)
                {
                    var infoVM = new MessageVM()
                    {
                        IsSuccess = true,
                        OccurredAt = _Clock.Now,
                        Text = string.Concat("Added ", oObjects.Count, " items to the collection")
                    };
                    messages.Insert(0, infoVM);
                }
                return new GenericResponse()
                {
                    Passed = true,
                    Modifications = _ModificationsBuilder.BuildFrom(_ObserverCache.GetAllObservers(), startedAt),
                    Messages = messages.ToArray()
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

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = new MessageVM[] { errorVM }
                };
            }
        }
    }
}