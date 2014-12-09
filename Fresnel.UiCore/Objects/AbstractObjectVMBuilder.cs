using Envivo.Fresnel.Core.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class AbstractObjectVMBuilder
    {
        private ObserverCache _ObserverCache;
        private PropertyVmBuilder _PropertyVmBuilder;

        public AbstractObjectVMBuilder
            (
            ObserverCache observerCache,
            PropertyVmBuilder propertyVmBuilder
            )
        {
            _ObserverCache = observerCache;
            _PropertyVmBuilder = propertyVmBuilder;
        }

        public ObjectVM BuildFor(BaseObjectObserver observer)
        {
            var oCollection = observer as CollectionObserver;
            var oObject = observer as ObjectObserver;

            if (oCollection != null)
            {
                return this.BuildForCollection(oCollection);
            }
            else if (oObject != null)
            {
                return this.BuildForObject(oObject);
            }

            return null;
        }

        private ObjectVM BuildForCollection(CollectionObserver oCollection)
        {
            var result = new CollectionVM()
            {
                ID = oCollection.ID,
                Name = oCollection.Template.FriendlyName,
                IsVisible = oCollection.Template.IsVisible,
                Description = oCollection.Template.Summary,
                Properties = this.CreateProperties(oCollection),
                Items = this.CreateItems(oCollection)
            };

            return result;
        }

        private IEnumerable<ObjectVM> CreateItems(CollectionObserver oCollection)
        {
            var items = new List<ObjectVM>();
            foreach (var obj in oCollection.GetContents())
            {
                var oObject = (ObjectObserver)_ObserverCache.GetObserver(obj);
                var objVM = this.BuildForObject(oObject);
                items.Add(objVM);
            }
            return items;
        }

        private ObjectVM BuildForObject(ObjectObserver oObject)
        {
            var result = new ObjectVM()
            {
                ID = oObject.ID,
                Name = oObject.Template.FriendlyName,
                IsVisible = oObject.Template.IsVisible,
                Description = oObject.Template.Summary,
                Properties = this.CreateProperties(oObject),
            };

            return result;
        }

        private IEnumerable<PropertyVM> CreateProperties(ObjectObserver oObject)
        {
            var properties = new List<PropertyVM>();
            foreach (var oProp in oObject.Properties.Values)
            {
                var propVM = _PropertyVmBuilder.BuildFor(oObject, oProp);
                properties.Add(propVM);
            }
            return properties;
        }

    }
}
