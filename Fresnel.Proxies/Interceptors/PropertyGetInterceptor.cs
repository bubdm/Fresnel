using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies
{

    public class PropertyGetInterceptor : IInterceptor, IDisposable
    {
        public PropertyGetInterceptor()
        {
        }

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

                this.PostInvoke(oProperty, invocation.ReturnValue);
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
            var tProperty = oProperty.TemplateAs<PropertyTemplate>();
            return tProperty.IsNonReference ?
                    true :
                    oProperty.IsLazyLoaded;
        }

        private void PostInvoke(ObjectPropertyObserver oProperty, object returnValue)
        {
            if (returnValue == null)
                return;

            var oReturnValue = (ObjectObserver)((IFresnelProxy)this.ProxyCache.GetProxy(returnValue)).Meta;
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
