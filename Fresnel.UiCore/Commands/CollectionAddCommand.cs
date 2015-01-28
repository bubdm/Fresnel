using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CollectionAddCommand
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

        public GenericResponse Invoke(CollectionRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var oCollection = (CollectionObserver)_ObserverCache.GetObserverById(request.CollectionID);
                if (oCollection == null)
                    throw new UiCoreException("Cannot find collection with ID " + request.CollectionID);

                ObjectObserver oObject = null;

                var isCreationRequired = request.ElementTypeName.IsNotEmpty();

                if (isCreationRequired)
                {
                    var tClass = _TemplateCache.GetTemplate(request.ElementTypeName);
                    if (tClass != null)
                    {
                        oObject = (ObjectObserver)_CreateObjectCommand.Invoke(tClass.RealType, null);
                        if (oObject == null)
                            throw new UiCoreException("Cannot create object of type " + tClass.FriendlyName);
                    }
                }
                else
                {
                    oObject = (ObjectObserver)_ObserverCache.GetObserverById(request.ElementID);
                    if (oObject == null)
                        throw new UiCoreException("Cannot find object with ID " + request.ElementID);
                }

                var oResult = _AddToCollectionCommand.Invoke(oCollection, oObject);

                // Other objects may have been affected by the action:
                _ObserverCache.ScanForChanges();

                var infoVM = new MessageVM()
                {
                    IsSuccess = true,
                    OccurredAt = _Clock.Now,
                    Text = isCreationRequired ?
                           string.Concat("Created and added new ", oObject.Template.FriendlyName) :
                           string.Concat("Added ", oObject.RealObject.ToString(), " to the collection")
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
    }
}