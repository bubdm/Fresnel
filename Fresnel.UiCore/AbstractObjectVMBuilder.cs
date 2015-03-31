using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class AbstractObjectVmBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private ObserverCache _ObserverCache;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;
        private EmptyPropertyVmBuilder _EmptyPropertyVmBuilder;
        private PropertyVmBuilder _PropertyVmBuilder;
        private MethodVmBuilder _MethodVmBuilder;

        public AbstractObjectVmBuilder
            (
            RealTypeResolver realTypeResolver,
            ObserverCache observerCache,
            ClassHierarchyBuilder classHierarchyBuilder,
            EmptyPropertyVmBuilder emptyPropertyVmBuilder,
            PropertyVmBuilder propertyVmBuilder,
            MethodVmBuilder methodVmBuilder
            )
        {
            _RealTypeResolver = realTypeResolver;
            _ObserverCache = observerCache;
            _ClassHierarchyBuilder = classHierarchyBuilder;
            _EmptyPropertyVmBuilder = emptyPropertyVmBuilder;
            _PropertyVmBuilder = propertyVmBuilder;
            _MethodVmBuilder = methodVmBuilder;
        }

        public ObjectVM BuildFor(BaseObjectObserver observer)
        {
            var oCollection = observer as CollectionObserver;
            var oObject = observer as ObjectObserver;

            if (oCollection != null)
            {
                // The View needs to know about ALL properties for all sub-classes of the Collection's element type:
                var tElement = oCollection.Template.InnerClass;
                return this.BuildForCollection(oCollection, tElement);
            }
            else if (oObject != null)
            {
                return this.BuildForObject(oObject);
            }

            return null;
        }

        public CollectionVM BuildForCollection(CollectionObserver oCollection, ClassTemplate tElement)
        {
            var allKnownProperties = _ClassHierarchyBuilder
                                        .GetProperties(tElement)
                                        .Where(p => !p.IsFrameworkMember &&
                                                     p.IsVisible);

            var elementProperties = new List<SettableMemberVM>();
            foreach (var tProp in allKnownProperties)
            {
                var propVM = _EmptyPropertyVmBuilder.BuildFor(tProp);
                propVM.Index = elementProperties.Count;
                elementProperties.Add(propVM);
            }

            var result = new CollectionVM()
            {
                ID = oCollection.ID,
                Name = oCollection.Template.FriendlyName,
                Type = oCollection.Template.RealType.Name,
                ElementType = tElement.RealType.FullName,
                IsVisible = oCollection.Template.IsVisible,
                ElementProperties = elementProperties,
                Description = oCollection.Template.XmlComments.Summary,
                Properties = this.CreateProperties(oCollection),
                Methods = this.CreateMethods(oCollection),
                Items = this.CreateItems(oCollection, allKnownProperties),

                DirtyState = this.CreateDirtyState(oCollection),
            };

            this.TrimRedundantContentFrom(result);

            return result;
        }

        private void TrimRedundantContentFrom(CollectionVM collectionVM)
        {
            foreach (var item in collectionVM.Items)
            {
                item.Description = null;
                foreach (var prop in item.Properties)
                {
                    prop.Description = null;
                }
                foreach (var method in item.Methods)
                {
                    method.Description = null;
                }
            }
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

                items.Add(objVM);
            }
            return items;
        }

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

                IsPersistable = oObject.Template.IsPersistable,
                DirtyState = this.CreateDirtyState(oObject),
            };

            return result;
        }

        private IEnumerable<SettableMemberVM> CreateProperties(ObjectObserver oObject)
        {
            var visibleProperties = oObject.Properties.Values.Where(p => !p.Template.IsFrameworkMember &&
                                                                          p.Template.IsVisible);

            var properties = new List<SettableMemberVM>();
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

        private DirtyStateVM CreateDirtyState(ObjectObserver oObject)
        {
            return new DirtyStateVM()
            {
                IsTransient = oObject.ChangeTracker.IsTransient,
                IsPersistent = oObject.ChangeTracker.IsPersistent,
                IsDirty = oObject.ChangeTracker.IsDirty,
                HasDirtyChildren = oObject.ChangeTracker.HasDirtyObjectGraph,
            };
        }

    }
}