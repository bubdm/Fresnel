using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
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

        public MethodVM BuildFor(MethodObserver oMethod)
        {
            var invokeCheck = _CanInvokeMethodPermission.IsSatisfiedBy(oMethod);

            var tMethod = oMethod.Template;
            var methodVM = new MethodVM()
            {
                ObjectID = oMethod.OuterObject.ID,
                Name = tMethod.FriendlyName,
                InternalName = tMethod.Name,
                Description = tMethod.XmlComments.Summary,
                Parameters = this.CreateParametersFor(tMethod).ToArray(),
                IsVisible = !tMethod.IsFrameworkMember && tMethod.IsVisible,
                IsEnabled = invokeCheck == null,
                Error = invokeCheck == null ? null : invokeCheck.ToSingleMessage()
            };

            return methodVM;
        }

        private IEnumerable<ParameterVM> CreateParametersFor(MethodTemplate tMethod)
        {
            var results = new List<ParameterVM>();

            foreach (var tParam in tMethod.Parameters.Values)
            {
                if (!tParam.IsVisible)
                    continue;

                var paramVM = _AbstractParameterVmBuilder.BuildFor(tParam);
                results.Add(paramVM);
            }

            return results;
        }
    }
}