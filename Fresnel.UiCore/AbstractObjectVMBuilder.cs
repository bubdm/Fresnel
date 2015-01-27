using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class AbstractObjectVMBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private ObserverCache _ObserverCache;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;
        private AbstractPropertyVmBuilder _PropertyVmBuilder;
        private MethodVmBuilder _MethodVmBuilder;

        public AbstractObjectVMBuilder
            (
            RealTypeResolver realTypeResolver,
            ObserverCache observerCache,
            ClassHierarchyBuilder classHierarchyBuilder,
            AbstractPropertyVmBuilder propertyVmBuilder,
            MethodVmBuilder methodVmBuilder
            )
        {
            _RealTypeResolver = realTypeResolver;
            _ObserverCache = observerCache;
            _ClassHierarchyBuilder = classHierarchyBuilder;
            _PropertyVmBuilder = propertyVmBuilder;
            _MethodVmBuilder = methodVmBuilder;
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
            // The View needs to know about ALL properties for all sub-classes of the Collection's element type:
            var tElement = oCollection.Template.InnerClass;
            var allKnownProperties = _ClassHierarchyBuilder.GetProperties(tElement)
                                        .Where(p => !p.IsFrameworkMember &&
                                                     p.IsVisible);

            var elementProperties = new List<ValueVM>();
            foreach (var prop in allKnownProperties)
            {
                var propVM = _PropertyVmBuilder.BuildFor(prop);
                propVM.Index = elementProperties.Count;
                elementProperties.Add(propVM);
            }

            var result = new CollectionVM()
            {
                ID = oCollection.ID,
                Name = oCollection.Template.FriendlyName,
                Type = oCollection.Template.RealType.Name,
                ElementType = oCollection.Template.InnerClass.RealType.FullName,
                IsVisible = oCollection.Template.IsVisible,
                ElementProperties = elementProperties,
                Description = oCollection.Template.XmlComments.Summary,
                Properties = this.CreateProperties(oCollection),
                Methods = this.CreateMethods(oCollection),
                Items = this.CreateItems(oCollection, allKnownProperties)
            };

            return result;
        }

        private IEnumerable<ObjectVM> CreateItems(CollectionObserver oCollection,
                                                  IEnumerable<PropertyTemplate> allKnownProperties)
        {
            var items = new List<ObjectVM>();
            foreach (var obj in oCollection.GetItems())
            {
                var objType = _RealTypeResolver.GetRealType(obj);

                var oObject = (ObjectObserver)_ObserverCache.GetObserver(obj, objType);
                var objVM = this.BuildForObject(oObject);

                //this.InsertUnallocatedProperties(objVM, allKnownProperties);

                items.Add(objVM);
            }
            return items;
        }

        //private void InsertUnallocatedProperties(ObjectVM objVM, IEnumerable<PropertyTemplate> allKnownProperties)
        //{
        //    var allocatedProperties = objVM.Properties.ToList();
        //    var allocatedPropertyNames = objVM.Properties.Select(p => p.PropertyName);

        //    var unallocatedProperties = allKnownProperties.Where(p => !allocatedPropertyNames.Contains(p.Name));
        //    foreach (var tProp in unallocatedProperties)
        //    {
        //        var propVM = new PropertyVM()
        //        {
        //            Name = tProp.FriendlyName,
        //            Index = allocatedProperties.Count,
        //            PropertyName = tProp.Name,
        //            IsRequired = tProp.IsNonReference && !tProp.IsNullableType,
        //            IsEnabled = false,
        //            CanWrite = false,
        //        };
        //        allocatedProperties.Add(propVM);
        //    }

        //    objVM.Properties = allocatedProperties;
        //}

        private ObjectVM BuildForObject(ObjectObserver oObject)
        {
            var title = oObject.RealObject.ToString() ?? oObject.Template.FriendlyName;
            if (title == oObject.Template.FullName)
            {
                title = oObject.Template.FriendlyName;
            }

            var result = new ObjectVM()
            {
                ID = oObject.ID,
                Name = title,
                Type = oObject.Template.RealType.Name,
                IsVisible = oObject.Template.IsVisible,
                Description = oObject.Template.XmlComments.Summary,
                Properties = this.CreateProperties(oObject),
                Methods = this.CreateMethods(oObject),
            };

            return result;
        }

        private IEnumerable<ValueVM> CreateProperties(ObjectObserver oObject)
        {
            var visibleProperties = oObject.Properties.Values.Where(p => !p.Template.IsFrameworkMember &&
                                                                          p.Template.IsVisible);

            var properties = new List<ValueVM>();
            foreach (var oProp in visibleProperties)
            {
                var propVM = _PropertyVmBuilder.BuildFor(oProp);
                propVM.Index = properties.Count;
                properties.Add(propVM);
            }
            return properties;
        }

        private IEnumerable<MethodVM> CreateMethods(ObjectObserver oObject)
        {
            var visibleMethods = oObject.Methods.Values.Where(m => !m.Template.IsFrameworkMember &&
                                                                    m.Template.IsVisible);

            var methods = new List<MethodVM>();
            foreach (var oMethod in visibleMethods)
            {
                var methodVM = _MethodVmBuilder.BuildFor(oObject, oMethod);
                methodVM.Index = methods.Count;
                methods.Add(methodVM);
            }
            return methods;
        }

        // TODO: Populate Add and Remove collection interactions:

        //private InteractionPoint CreateAdd(BasePropertyObserver oProp, object propertyValue)
        //{
        //    var tProp = oProp.Template;
        //    var result = new InteractionPoint()
        //    {
        //        IsEnabled = tProp.CanAdd &&
        //                    propertyValue != null &&
        //                    tProp.IsCollection
        //    };
        //    return result;
        //}

        //private InteractionPoint CreateRemove(BasePropertyObserver oProp, object propertyValue)
        //{
        //    var tProp = oProp.Template;
        //    var result = new InteractionPoint()
        //    {
        //        IsEnabled = tProp.CanRemove &&
        //                    propertyValue != null &&
        //                    tProp.IsCollection
        //    };
        //    return result;
        //}

    }
}