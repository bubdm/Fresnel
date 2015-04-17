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
        private Core.Commands.GetPropertyCommand _GetPropertyCommand;
        private Core.Commands.CreateObjectCommand _CreateObjectCommand;
        private Func<ObjectPropertyObserver, ObjectObserver, AddToCollectionEvent> _AddToCollectionEventFactory;
        private EventTimeLine _EventTimeLine;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private ModificationsVmBuilder _ModificationsBuilder;
        private IClock _Clock;

        public CollectionAddCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            Core.Commands.GetPropertyCommand getPropertyCommand,
            Core.Commands.CreateObjectCommand createObjectCommand,
            Func<ObjectPropertyObserver, ObjectObserver, AddToCollectionEvent> addToCollectionEventFactory,
            EventTimeLine eventTimeLine,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _GetPropertyCommand = getPropertyCommand;
            _CreateObjectCommand = createObjectCommand;
            _AddToCollectionEventFactory = addToCollectionEventFactory;
            _EventTimeLine = eventTimeLine;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _Clock = clock;
        }

        public GenericResponse Invoke(CollectionAddNewRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oParent = this.GetObserver(request.ParentObjectID);
                var oProp = (ObjectPropertyObserver)oParent.Properties[request.CollectionPropertyName];
                var oCollection = GetCollectionObserver(oParent, oProp);

                ObjectObserver oObject = null;
                var tClass = _TemplateCache.GetTemplate(request.ElementTypeName);
                if (tClass != null)
                {
                    oObject = (ObjectObserver)_CreateObjectCommand.Invoke(tClass.RealType, oParent.RealObject);
                    if (oObject == null)
                        throw new UiCoreException("Cannot create object of type " + tClass.FriendlyName);
                }

                var addEvent = _AddToCollectionEventFactory(oProp, oObject);
                var addOperation = (ActionResult<ObjectObserver>)addEvent.Do();
                if (addOperation.Failed)
                {
                    throw addOperation.FailureException;
                }

                _EventTimeLine.Add(addEvent);

                var oResult = addOperation.Result;

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

                var oParent = this.GetObserver(request.ParentObjectID);
                var oProp = (ObjectPropertyObserver)oParent.Properties[request.CollectionPropertyName];
                var oCollection = this.GetCollectionObserver(oParent, oProp);

                var oObjects = new List<ObjectObserver>();
                foreach (var elementID in request.ElementIDs)
                {
                    var oObject = this.GetObserver(elementID);

                    var addEvent = _AddToCollectionEventFactory(oProp, oObject);
                    _EventTimeLine.Add(addEvent);

                    var addOperation = (ActionResult<ObjectObserver>)addEvent.Do();
                    if (addOperation.Passed)
                    {
                        oObjects.Add(oObject);

                        var infoVM = new MessageVM()
                        {
                            IsInfo = true,
                            OccurredAt = _Clock.Now,
                            Text = string.Concat("Added ", oObject.RealObject.ToString(), " to the collection")
                        };
                        messages.Add(infoVM);
                    }
                    else if (addOperation.Failed)
                    {
                        var infoVM = new MessageVM()
                        {
                            IsError = true,
                            OccurredAt = _Clock.Now,
                            Text = string.Concat("Unable to add ", oObject.RealObject.ToString(), " to the collection")
                        };
                        messages.Add(infoVM);
                    }
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

        private ObjectObserver GetObserver(Guid objectID)
        {
            var oObject = (ObjectObserver)_ObserverCache.GetObserverById(objectID);
            if (oObject == null)
                throw new UiCoreException("Cannot find object for " + objectID);
            return oObject;
        }

        private CollectionObserver GetCollectionObserver(ObjectObserver oParent, ObjectPropertyObserver oProp)
        {
            var oCollection = (CollectionObserver)_GetPropertyCommand.Invoke(oProp);
            if (oCollection == null)
                throw new UiCoreException("Cannot find collection for " + oProp.Template.Name);
            return oCollection;
        }

    }
}