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
        private Core.Commands.AddToCollectionCommand _AddToCollectionCommand;
        private AbstractObjectVmBuilder _ObjectVMBuilder;
        private ModificationsVmBuilder _ModificationsBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public CollectionAddCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            Core.Commands.GetPropertyCommand getPropertyCommand,
            Core.Commands.CreateObjectCommand createObjectCommand,
            Core.Commands.AddToCollectionCommand addToCollectionCommand,
            AbstractObjectVmBuilder objectVMBuilder,
            ModificationsVmBuilder modificationsBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
            )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _GetPropertyCommand = getPropertyCommand;
            _CreateObjectCommand = createObjectCommand;
            _AddToCollectionCommand = addToCollectionCommand;
            _ObjectVMBuilder = objectVMBuilder;
            _ModificationsBuilder = modificationsBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
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

                var oResult = _AddToCollectionCommand.Invoke(oProp, oCollection, oObject);

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
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = errorVMs
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

                    try
                    {
                        var oResult = _AddToCollectionCommand.Invoke(oProp, oCollection, oObject);

                        var infoVM = new MessageVM()
                        {
                            IsInfo = true,
                            OccurredAt = _Clock.Now,
                            Text = string.Concat("Added ", oObject.RealObject.ToString(), " to the collection")
                        };
                        messages.Add(infoVM);
                    }
                    catch (Exception ex)
                    {
                        var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);
                        messages.AddRange(errorVMs);
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
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GenericResponse()
                {
                    Failed = true,
                    Messages = errorVMs
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