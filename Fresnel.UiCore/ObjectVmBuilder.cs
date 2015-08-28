using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class ObjectVmBuilder
    {
        private RealTypeResolver _RealTypeResolver;
        private ObserverRetriever _ObserverRetriever;
        private ClassHierarchyBuilder _ClassHierarchyBuilder;
        private EmptyPropertyVmBuilder _EmptyPropertyVmBuilder;
        private PropertyVmBuilder _PropertyVmBuilder;
        private MethodVmBuilder _MethodVmBuilder;
        private DirtyStateVmBuilder _DirtyStateVmBuilder;

        public ObjectVmBuilder
            (
            RealTypeResolver realTypeResolver,
            ObserverRetriever observerRetriever,
            ClassHierarchyBuilder classHierarchyBuilder,
            EmptyPropertyVmBuilder emptyPropertyVmBuilder,
            PropertyVmBuilder propertyVmBuilder,
            MethodVmBuilder methodVmBuilder,
            DirtyStateVmBuilder dirtyStateVmBuilder
            )
        {
            _RealTypeResolver = realTypeResolver;
            _ObserverRetriever = observerRetriever;
            _ClassHierarchyBuilder = classHierarchyBuilder;
            _EmptyPropertyVmBuilder = emptyPropertyVmBuilder;
            _PropertyVmBuilder = propertyVmBuilder;
            _MethodVmBuilder = methodVmBuilder;
            _DirtyStateVmBuilder = dirtyStateVmBuilder;
        }

        public ObjectVM BuildFor(ObjectObserver oObject)
        {
            var allKnownProperties = oObject.Template.Properties.VisibleOnly;
            return this.BuildFor(oObject, allKnownProperties);
        }

        public ObjectVM BuildFor(ObjectObserver oObject, IEnumerable<PropertyTemplate> allKnownProperties)
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
                Properties = this.CreateProperties(oObject, allKnownProperties).ToArray(),
                Methods = this.CreateMethods(oObject).ToArray(),

                IsPersistable = oObject.Template.IsPersistable,
                DirtyState = _DirtyStateVmBuilder.BuildFor(oObject),
                IsPinned = oObject.IsPinned
            };

            return result;
        }

        private IEnumerable<PropertyVM> CreateProperties(ObjectObserver oObject, IEnumerable<PropertyTemplate> allKnownProperties)
        {
            var properties = new List<PropertyVM>();
            foreach (var tProp in allKnownProperties)
            {
                var oProp = oObject.Properties.TryGetValueOrNull(tProp.Name);
                var propVM = oProp != null ?
                            _PropertyVmBuilder.BuildFor(oProp) :
                            _PropertyVmBuilder.BuildEmptyVMFor(tProp);
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
                var methodVM = _MethodVmBuilder.BuildFor(oMethod);
                methodVM.Index = methods.Count;
                methods.Add(methodVM);
            }
            return methods;
        }

    }
}