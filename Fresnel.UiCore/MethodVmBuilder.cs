using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class MethodVmBuilder
    {
        private AbstractParameterVmBuilder _AbstractParameterVmBuilder;
        private CanInvokeMethodPermission _CanInvokeMethodPermission;

        public MethodVmBuilder
            (
            AbstractParameterVmBuilder abstractParameterVmBuilder,
            CanInvokeMethodPermission canInvokeMethodPermission
            )
        {
            _AbstractParameterVmBuilder = abstractParameterVmBuilder;
            _CanInvokeMethodPermission = canInvokeMethodPermission;
        }

        public ObjectMethodVM BuildFor(MethodObserver oMethod)
        {
            var invokeCheck = _CanInvokeMethodPermission.IsSatisfiedBy(oMethod);

            var methodVM = this.BuildFor(oMethod.Template);
            methodVM.ObjectID = oMethod.OuterObject.ID;
            methodVM.IsEnabled = invokeCheck == null;
            methodVM.Error = invokeCheck == null ? null : invokeCheck.Flatten().Message;

            return methodVM;
        }

        public ObjectMethodVM BuildFor(MethodTemplate tMethod)
        {
            var methodVM = new ObjectMethodVM()
            {
                Name = tMethod.FriendlyName,
                InternalName = tMethod.Name,
                Description = tMethod.XmlComments.Summary,
                Parameters = this.CreateParametersFor(tMethod).ToArray(),
                IsVisible = !tMethod.IsFrameworkMember && tMethod.IsVisible,
            };

            return methodVM;
        }

        public DependencyMethodVM BuildFor(ClassTemplate tClass, MethodTemplate tMethod)
        {
            var methodVM = new DependencyMethodVM()
            {
                ClassType = tClass.RealType.FullName,
                Name = tMethod.FriendlyName,
                InternalName = tMethod.Name,
                Description = tMethod.XmlComments.Summary,
                Parameters = this.CreateParametersFor(tMethod).ToArray(),
                IsVisible = !tMethod.IsFrameworkMember && tMethod.IsVisible,
                IsEnabled = true,
            };

            return methodVM;
        }

        private IEnumerable<ParameterVM> CreateParametersFor(MethodTemplate tMethod)
        {
            var results = new List<ParameterVM>();

            foreach (var tParam in tMethod.Parameters.Values)
            {
                var paramVM = _AbstractParameterVmBuilder.BuildFor(tParam);
                results.Add(paramVM);
            }

            return results;
        }
    }
}