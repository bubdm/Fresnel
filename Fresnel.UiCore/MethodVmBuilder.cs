using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
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

        public MethodVM BuildFor(ObjectObserver oObject, MethodObserver oMethod)
        {
            var invokeCheck = _CanInvokeMethodPermission.IsSatisfiedBy(oMethod);

            var methodVM = new MethodVM()
            {
                ObjectID = oObject.ID,
                Name = oMethod.Template.FriendlyName,
                InternalName = oMethod.Template.Name,
                Description = oMethod.Template.XmlComments.Summary,
                Parameters = this.CreateParametersFor(oMethod).ToArray(),
                //IsAsync = oMethod.Template.Configurations.Get<MethodConfiguration>().IsAsynchronous,
                IsVisible = !oMethod.Template.IsFrameworkMember && oMethod.Template.IsVisible,
                IsEnabled = invokeCheck == null,
                Error = invokeCheck == null ? null : invokeCheck.Flatten().Message
            };

            return methodVM;
        }

        private IEnumerable<ParameterVM> CreateParametersFor(MethodObserver oMethod)
        {
            var results = new List<ParameterVM>();

            foreach (var tParam in oMethod.Template.Parameters.Values)
            {
                var paramVM = _AbstractParameterVmBuilder.BuildFor(tParam);
                results.Add(paramVM);
            }

            return results;
        }
    }
}