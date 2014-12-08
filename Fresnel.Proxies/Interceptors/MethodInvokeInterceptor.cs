using Castle.DynamicProxy;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies.Interceptors
{


    public class MethodInvokeInterceptor : IInterceptor
    {
        public MethodInvokeInterceptor()
        {
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name);
            var oObject = ((IFresnelProxy)invocation.Proxy).Meta;
            var oMethod = oObject.Methods.TryGetValueOrNull(invocation.Method.Name);

            var isDomainObjectMethod = false;
            if (oMethod != null)
            {
                var tClass = oMethod.OuterObject.Template;
                var objectAttr = tClass.Attributes.Get<ObjectInstanceAttribute>();
                isDomainObjectMethod = (objectAttr.HasHiddenMemberNamed(invocation.Method.Name) == false);
            }

            if (isDomainObjectMethod == true)
            {
                this.PreInvoke(oMethod);

                invocation.Proceed();

                this.PostInvoke(invocation, oMethod);
            }
            else
            {
                // In case we don't recognise the operation:
                invocation.Proceed();
            }
        }

        private void PreInvoke(MethodObserver oMethod)
        {
            // As a rule, methods can only be invoked if there are no pending changes to *any* objects in the ObserverCache.
            // This is to ensure that the client and the Persistor don't get out of sync:
            var tClass = oMethod.OuterObject.Template;

            var allowWithUnsavedObjects = tClass.Attributes.Get<MethodAttribute>().AllowWithUnsavedObjects;
            if (allowWithUnsavedObjects == false)
            {
                var check = this.AllObjectsAreNotDirty(oMethod.OuterObject);
                if (check.Failed)
                {
                    var msg = string.Format("Action '{0}' cannot be invoked until all changes are saved. Please save everything and try again.", oMethod.FullName);
                    throw new MethodAccessException(msg);
                }
            }

            //var invokeCheck = oMethod.Permissions.Invoke.Check();
            //if (invokeCheck.Failed)
            //{
            //    oMethod.ErrorMessage = invokeCheck.FailureReason;
            //    throw new SecurityException(invokeCheck.FailureReason);
            //}

            //oMethod.OuterObject.Persistor.WakeUp();

            oMethod.LastInvokedAtUtc = DateTime.UtcNow;
        }

        private IAssertion AllObjectsAreNotDirty(ObjectObserver oObject)
        {
            var areDirtyObjectsPresent = false;

            //var graphIterator = new ObjectGraphIterator();
            //foreach (var oObj in graphIterator.GetObjects(oObject, ObjectGraphIterator.TraversingOptions.IncludeObjectsMarkedForDeletion))
            //{
            //    if (oObj.ChangeTracker.IsDirty)
            //    {
            //        areDirtyObjectsPresent = true;
            //        break;
            //    }
            //}

            return areDirtyObjectsPresent ?
                   Assertion.Fail("Dirty Domain Objects are connected to this Object") :
                   Assertion.Pass();
        }

        private void PostInvoke(IInvocation invocation, MethodObserver oMethod)
        {
            //this.DetectChangesInAssociatedObservers(invocation);

            this.MarkChangedObjects(oMethod);

            if (invocation.ReturnValue != null)
            {
                var returnValueProxy = (IFresnelProxy)this.ProxyCache.GetProxy(invocation.ReturnValue);
                invocation.ReturnValue = returnValueProxy;
            }

            //oMethod.OuterObject.Persistor.Suspend();
        }

        private void MarkChangedObjects(MethodObserver oMethod)
        {
            //// See if any connected objects were changed:
            //var traversingOptions = ObjectGraphIterator.TraversingOptions.IncludeObjectsMarkedForDeletion | ObjectGraphIterator.TraversingOptions.IncludeParentProperties;
            //foreach (var oObject in new ObjectGraphIterator().GetObjects(oMethod.OuterObject, traversingOptions))
            //{
            //    // CODE SMELL: This should be in a separate method, but this is more optimal (less graph traversals):
            //    oObject.UI.Style.Update();

            //    // We're only interested in things that were affected AFTER the method invocation:
            //    if (oObject.WasCreatedBefore(oMethod.LastInvokedAtUtc) == false)
            //    {
            //        oObject.ChangeTracker.DetectChangesSince(oMethod.LastInvokedAtUtc);
            //    }
            //}
        }

        private void DetectChangesInAssociatedObservers(IInvocation invocation)
        {
            //BaseObjectObserver oTemp;

            //if (invocation.InvocationTarget != null)
            //{
            //    oTemp = this.ProxyCache.GetObserverForProxyUse(invocation.InvocationTarget);
            //    oTemp.ChangeTracker.DetectChanges();
            //}

            //if (invocation.Arguments.Length > 0)
            //{
            //    for (var i = 0; i < invocation.Arguments.Length; i++)
            //    {
            //        var argValue = invocation.GetArgumentValue(i);
            //        if (argValue == null)
            //            continue;

            //        oTemp = this.ProxyCache.GetObserverForProxyUse(invocation.GetArgumentValue(i));
            //        oTemp.ChangeTracker.DetectChanges();
            //    }
            //}
        }


        public override bool Equals(object obj)
        {
            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public void Dispose()
        {
            this.ProxyCache = null;
        }

    }

}
