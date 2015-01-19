using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.UiCore.Model;

namespace Envivo.Fresnel.UiCore
{
    public class MethodVmBuilder
    {
        private CanInvokeMethodPermission _CanInvokeMethodPermission;

        public MethodVmBuilder
            (
            CanInvokeMethodPermission canInvokeMethodPermission
            )
        {
            _CanInvokeMethodPermission = canInvokeMethodPermission;
        }

        public MethodVM BuildFor(ObjectObserver oObject, MethodObserver oMethod)
        {
            var invokeCheck = _CanInvokeMethodPermission.IsSatisfiedBy(oMethod);

            var methodVM = new MethodVM()
            {
                ObjectID = oObject.ID,
                Name = oMethod.Template.FriendlyName,
                MethodName = oMethod.Template.Name,
                Description = oMethod.Template.XmlComments.Summary,
                IsAsync = oMethod.Template.Attributes.Get<MethodAttribute>().IsAsynchronous,
                IsVisible = !oMethod.Template.IsFrameworkMember && oMethod.Template.IsVisible,
            };

            if (invokeCheck.Passed)
            {
                methodVM.IsEnabled = true;
            }
            else
            {
                methodVM.Error = invokeCheck.FailureReason;
            }

            return methodVM;
        }
    }
}