using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class PropertyGetInterceptor : IInterceptor, IDisposable
    {
        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name); 
            
            var oObject = ((IFresnelProxy)invocation.Proxy).Meta;

            var propertyName = invocation.Method.Name.Remove(0, 4);
            var oProperty = oObject.Properties.TryGetValueOrNull(propertyName) as ObjectPropertyObserver;
            if (oProperty != null)
            {
                this.PreInvoke(oProperty);

                if (IsPropertyAccessible(oProperty))
                {
                    invocation.Proceed();
                }

                this.PostInvoke(invocation, oProperty);
            }
            else
            {
                // In case we don't recognise the operation:
                invocation.Proceed();
            }
        }

        private void PreInvoke(BasePropertyObserver oProperty)
        {
            //if (oProperty == null)
            //    return;

            //var readCheck = oProperty.Permissions.Read.Check();
            //if (readCheck.Failed)
            //{
            //    oProperty.ErrorMessage = readCheck.FailureReason;
            //    throw new SecurityException(readCheck.FailureReason);
            //}

            //if (oProperty.IsReferenceType)
            //{
            //    this.ApplyLazyLoadingBehaviourTo((ObjectPropertyObserver)oProperty);
            //}

            oProperty.LastAccessedAtUtc = DateTime.UtcNow;
        }

        private void ApplyLazyLoadingBehaviourTo(ObjectPropertyObserver oProperty)
        {
            if (oProperty.IsLazyLoaded)
                return;

            //if (this.ProxyCache.Configuration.IsAutoLoadingEnabled)
            //{
            //    // This should allow Nhibernate proxies to kick in:
            //    oProperty.IsLazyLoadPending = false;
            //}
        }

        /// <summary>
        /// Determines if the Property value is available for reading. Used to preent accidental lazy-loading.
        /// </summary>
        /// <param name="oProperty"></param>
        /// <returns></returns>
        private bool IsPropertyAccessible(ObjectPropertyObserver oProperty)
        {
            // TODO: An InMemoryDataStore should force IsLazyLoaded = TRUE
            return true;
            //var tProperty = oProperty.TemplateAs<PropertyTemplate>();
            //if (tProperty.IsReferenceType)
            //{
            //    return oProperty.IsLazyLoaded;
            //}
            //else
            //{
            //    return true;
            //}
        }

        private void PostInvoke(IInvocation invocation, ObjectPropertyObserver oProperty)
        {
            if (invocation.ReturnValue == null)
                return;

            // Make sure we're returning a proxy (so that it can be intercepted further):
            var returnValueProxy = (IFresnelProxy)this.ProxyCache.GetProxy(invocation.ReturnValue);
            invocation.ReturnValue = returnValueProxy;

            var oReturnValue = (ObjectObserver)(returnValueProxy).Meta;
            oReturnValue.AssociateWith(oProperty);
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
