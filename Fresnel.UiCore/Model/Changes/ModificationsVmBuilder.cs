using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;

using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore.Model.Changes
{
    public class ModificationsVmBuilder
    {
        private AbstractObjectVmBuilder _AbstractObjectVMBuilder;
        private PropertyStateVmBuilder _PropertyStateVmBuilder;
        private ParameterStateVmBuilder _ParameterStateVmBuilder;
        private DirtyStateVmBuilder _DirtyStateVmBuilder;
        private ObserverRetriever _ObserverRetriever;

        public ModificationsVmBuilder
            (
            AbstractObjectVmBuilder abstractObjectVMBuilder,
            PropertyStateVmBuilder propertyStateVmBuilder,
            ParameterStateVmBuilder parameterStateVmBuilder,
            DirtyStateVmBuilder dirtyStateVmBuilder,
            ObserverRetriever ObserverRetriever
            )
        {
            _AbstractObjectVMBuilder = abstractObjectVMBuilder;
            _PropertyStateVmBuilder = propertyStateVmBuilder;
            _ParameterStateVmBuilder = parameterStateVmBuilder;
            _DirtyStateVmBuilder = dirtyStateVmBuilder;
            _ObserverRetriever = ObserverRetriever;
        }

        public ModificationsVM BuildFrom(IEnumerable<ObjectObserver> observers, long startedAt)
        {
            var newObjects = observers
                                    .Select(o => o.ChangeTracker.GetObjectCreation())
                                    .Where(c => c.Sequence >= startedAt)
                                    .ToArray();

            var propertyChanges = observers
                                    .SelectMany(o => o.ChangeTracker.GetPropertyChangesSince(startedAt))
                                    .ToArray();

            var objectTitleChanges = observers
                                    .SelectMany(o => o.ChangeTracker.GetTitleChangesSince(startedAt))
                                    .ToArray();

            var collectionAdds = observers
                                    .OfType<CollectionObserver>()
                                    .SelectMany(c => c.ChangeTracker.GetCollectionAdditionsSince(startedAt))
                                    .ToArray();

            var collectionRemoves = observers
                                    .OfType<CollectionObserver>()
                                    .SelectMany(c => c.ChangeTracker.GetCollectionRemovalsSince(startedAt))
                                    .ToArray();

            var dirtyTrackers = observers
                                    .ToArray();

            var result = new ModificationsVM()
            {
                NewObjects = newObjects.Select(o => _AbstractObjectVMBuilder.BuildFor(o.Object)).ToArray(),
                PropertyChanges = propertyChanges.Select(c => CreatePropertyChange(c)).ToArray(),
                ObjectTitleChanges = objectTitleChanges.Select(c => CreateTitleChange(c)).ToArray(),
                CollectionAdditions = collectionAdds.Select(c => CreateCollectionElement(c)).ToArray(),
                CollectionRemovals = collectionRemoves.Select(c => CreateCollectionElement(c)).ToArray(),
                DirtyStateChanges = dirtyTrackers.Select(c => _DirtyStateVmBuilder.BuildFor(c)).ToArray(),
            };

            return result;
        }

        public ModificationsVM BuildFrom(ObjectPropertyObserver oProperty)
        {
            var propertyChange = new PropertyChange()
            {
                Property = oProperty,
            };

            var propertyChangeVM = this.CreatePropertyChange(propertyChange);

            var result = new ModificationsVM()
            {
                PropertyChanges = new PropertyChangeVM[] { propertyChangeVM }
            };

            return result;
        }

        public ModificationsVM BuildFrom(MethodObserver oMethod, ParameterObserver oParam)
        {
            var parameterChange = new ParameterChangeVM()
            {
                ObjectID = oMethod.OuterObject.ID,
                MethodName = oMethod.Template.Name,
                ParameterName = oParam.Template.Name,
                State = _ParameterStateVmBuilder.BuildFor(oParam)
            };

            var result = new ModificationsVM()
            {
                MethodParameterChanges = new ParameterChangeVM[] { parameterChange }
            };

            return result;
        }

        private PropertyChangeVM CreatePropertyChange(PropertyChange propertyChange)
        {
            var oProperty = propertyChange.Property;

            var result = new PropertyChangeVM()
            {
                ObjectID = oProperty.OuterObject.ID,
                PropertyName = oProperty.Template.Name,
                State = _PropertyStateVmBuilder.BuildFor(oProperty)
            };

            // As the s
            if (oProperty.Template.IsNonReference)
            {
                result.State.Value = propertyChange.NewValue;

                // HACK:
                if (oProperty.Template.PropertyType.IsEnum &&
                    result.State.Value != null)
                {
                    result.State.Value = (int)result.State.Value;
                }
            }
            else if (propertyChange.NewValue != null)
            {
                var oObj = _ObserverRetriever.GetObserver(propertyChange.NewValue, oProperty.Template.PropertyType);
                result.State.ReferenceValueID = oObj.ID;
            }
            else
            {
                result.State.FriendlyValue = "";
                result.State.Value = null;
                result.State.ReferenceValueID = null;
            }

            return result;
        }

        private ObjectTitleChangeVM CreateTitleChange(ObjectTitleChange titleChange)
        {
            var result = new ObjectTitleChangeVM()
            {
                ObjectID = titleChange.ObjectObserver.ID,
                Title = titleChange.NewValue
            };
            return result;
        }

        private CollectionElementVM CreateCollectionElement(CollectionAdd collectionAdd)
        {
            var oElement = _ObserverRetriever.GetObserver(collectionAdd.Element, collectionAdd.Collection.Template.InnerClass.RealType);

            var result = new CollectionElementVM()
            {
                CollectionId = collectionAdd.Collection.ID,
                ElementId = oElement.ID,
            };
            return result;
        }

        private CollectionElementVM CreateCollectionElement(CollectionRemove collectionRemove)
        {
            var oElement = _ObserverRetriever.GetObserver(collectionRemove.Element, collectionRemove.Collection.Template.InnerClass.RealType);

            var result = new CollectionElementVM()
            {
                CollectionId = collectionRemove.Collection.ID,
                ElementId = oElement.ID,
            };
            return result;
        }

    }
}