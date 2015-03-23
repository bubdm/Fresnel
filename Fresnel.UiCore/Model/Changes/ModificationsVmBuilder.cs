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
        private ObserverCache _ObserverCache;

        public ModificationsVmBuilder
            (
            AbstractObjectVmBuilder abstractObjectVMBuilder,
            PropertyStateVmBuilder _propertyStateVmBuilder,
            ObserverCache observerCache
            )
        {
            _AbstractObjectVMBuilder = abstractObjectVMBuilder;
            _PropertyStateVmBuilder = _propertyStateVmBuilder;
            _ObserverCache = observerCache;
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

            var collectionAdds = observers
                                    .OfType<CollectionObserver>()
                                    .SelectMany(c => c.ChangeTracker.GetCollectionAdditionsSince(startedAt))
                                    .ToArray();

            var collectionRemoves = observers
                                    .OfType<CollectionObserver>()
                                    .SelectMany(c => c.ChangeTracker.GetCollectionRemovalsSince(startedAt))
                                    .ToArray();

            var result = new ModificationsVM()
            {
                NewObjects = newObjects.Select(o => _AbstractObjectVMBuilder.BuildFor(o.Object)),
                PropertyChanges = propertyChanges.Select(c => CreatePropertyChange(c)),
                CollectionAdditions = collectionAdds.Select(c => CreateCollectionElement(c)),
                CollectionRemovals = collectionRemoves.Select(c => CreateCollectionElement(c)),
            };

            return result;
        }

        private PropertyChangeVM CreatePropertyChange(PropertyChange propertyChange)
        {
            var oProperty = propertyChange.Property;

            var result = new PropertyChangeVM()
            {
                ObjectId = oProperty.OuterObject.ID,
                PropertyName = oProperty.Template.Name,
                State = _PropertyStateVmBuilder.BuildFor(oProperty)
            };

            if (oProperty.Template.IsNonReference)
            {
                result.State.Value = propertyChange.NewValue;

                // HACK:
                if (oProperty.Template.PropertyType.IsEnum)
                {
                    result.State.Value = (int)result.State.Value;
                }
            }
            else if (propertyChange.NewValue != null)
            {
                var oObj = _ObserverCache.GetObserver(propertyChange.NewValue, oProperty.Template.PropertyType);
                result.State.ReferenceValueID = oObj.ID;
            }
            else
            {
                result.State.FriendlyValue = "";
            }

            return result;
        }

        private CollectionElementVM CreateCollectionElement(CollectionAdd collectionAdd)
        {
            var oElement = _ObserverCache.GetObserver(collectionAdd.Element, collectionAdd.Collection.Template.InnerClass.RealType);

            var result = new CollectionElementVM()
            {
                CollectionId = collectionAdd.Collection.ID,
                ElementId = oElement.ID,
            };
            return result;
        }

        private CollectionElementVM CreateCollectionElement(CollectionRemove collectionRemove)
        {
            var oElement = _ObserverCache.GetObserver(collectionRemove.Element, collectionRemove.Collection.Template.InnerClass.RealType);

            var result = new CollectionElementVM()
            {
                CollectionId = collectionRemove.Collection.ID,
                ElementId = oElement.ID,
            };
            return result;
        }
    }
}