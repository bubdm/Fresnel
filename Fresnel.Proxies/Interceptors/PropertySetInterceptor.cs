using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies
{


    public class PropertySetInterceptor : IInterceptor, IDisposable
    {
        private ObserverCache _ObserverCache;

        public PropertySetInterceptor(ObserverCache observerCache)
        {
            _ObserverCache = observerCache;
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name);

            var oObject = ((IFresnelProxy)invocation.Proxy).Meta;

            var oProperty = this.GetPropertyObserver(oObject, invocation.Method.Name);

            if (oProperty != null)
            {
                var oValue = this.GetValueObserver(invocation.Arguments[0]);

                //this.PreInvoke(invocation, oProperty, oValue);

                object originalValue = null;
                invocation.Proceed();

                this.PostInvoke(oProperty, oValue, originalValue);
            }
            else
            {
                // In case we don't recognise the operation:
                invocation.Proceed();
            }
        }

        private BasePropertyObserver GetPropertyObserver(ObjectObserver oObject, string methodName)
        {
            var result = oObject.Properties.TryGetValueOrNull(methodName.Remove(0, 4));
            return result;
        }

        private BaseObjectObserver GetValueObserver(object newValue)
        {
            if (newValue == null)
                return null;

            var type = newValue.GetType();
            if (type.IsNonReference())
                return null;

            var result = _ObserverCache.GetObserver(newValue);
            return result;
        }

        private void PreInvoke(IInvocation invocation, BasePropertyObserver oProperty, BaseObjectObserver oValue)
        {
            //// Check if the operation is valid for the "Aggregate Root" contraint:
            //if (My.Application.UnitOfWorkManager.IsPartOfAggregateRoot(My.Application.UI.ActiveWidget) == false)
            //{
            //    var msg = string.Format("'{0}.{1}' cannot be modified here. Please find it's owner and modify it there.", oProperty.OuterObject.FriendlyName, oProp.FriendlyName);
            //    throw new FresnelException(msg);
            //}

            //var writeCheck = oProperty.Permissions.Write.Check(oValue);
            //if (writeCheck.Failed)
            //{
            //    oProperty.ErrorMessage = writeCheck.FailureReason;
            //    throw new SecurityException(writeCheck.FailureReason);
            //}

            //var oObject = oProperty.OuterObject;
            //var isImmutable = oObject.Template.Attributes.Get<ObjectInstanceAttribute>().IsImmutable;
            //if (oObject.ChangeTracker.IsNewInstance == false && isImmutable)
            //{
            //    var msg = string.Format("'{0}' cannot be modifed once it has been saved", oObject.ToString(false, true, true), oObject);
            //    oProperty.ErrorMessage = msg;
            //    throw new FresnelException(msg);
            //}

            ////-----

            //var isValueObject = oValue != null &&
            //                    oValue.IsNull == false &&
            //                    oValue.IsValueObject;
            //if (isValueObject)
            //{
            //    // ValueObjects are cloned:
            //    invocation.SetArgumentValue(0, ((ObjectObserver)oValue).Clone().RealObject);
            //}

            ////-----

            //// Make the Persistor aware of the change that is about to happen:
            //oObject.Persistor.WakeUp();

            oProperty.LastAccessedAtUtc = DateTime.UtcNow;

            //originalValue = oProperty.PropertyTemplate.GetValue(oObject.RealObject);
        }

        private void PostInvoke(BasePropertyObserver oProperty, BaseObjectObserver oNewValue, object originalValue)
        {
            //var oObject = oProperty.OuterObject;

            //try
            //{
            //    var oOriginalValue = this.ProxyCache.GetObserverForProxyUse(originalValue);

            //    // We're replacing the value, so disassociate the existing value from this property:
            //    if (oOriginalValue != null)
            //    {
            //        oOriginalValue.DisassociateFrom(oProperty);
            //    }

            //    if (oNewValue == null)
            //    {
            //        var oOriginalObject = oOriginalValue as ObjectObserver;
            //        var canBeDeleted = oOriginalObject != null &&
            //                           oOriginalObject.IsOwnedBy(oObject);

            //        if (canBeDeleted)
            //        {
            //            // Composite objects can be deleted:
            //            var objectsToDelete = new ObjectObserverCollection();
            //            objectsToDelete.Add(oOriginalObject);
            //            oObject.Persistor.Delete(objectsToDelete);

            //            oOriginalObject.MakeOrphan();
            //        }
            //    }
            //    else
            //    {
            //        oNewValue.AssociateWith(oProperty);
            //    }

            //    oProperty.OuterObject.ChangeTracker.HasChanges = true;
            //}
            //catch (Exception ex)
            //{
            //    var message = this.CreateDescriptiveErrorMessage(oProperty, oNewValue, ex.Message);
            //    oProperty.ErrorMessage = message;

            //    throw new FresnelException(message, ex);
            //}
            //finally
            //{
            //    oObject.Persistor.Suspend();
            //}
        }

        private string CreateDescriptiveErrorMessage(BasePropertyObserver oProperty, BaseObjectObserver oNewValue, string errorMessage)
        {
            var tProp = oProperty.Template;
            var message = string.Format("Unable to set '{0}' to '{1}':",
                                        tProp.FriendlyName,
                                        oNewValue == null ? "null" : oNewValue.RealObject.ToString());
            return string.Concat(message, Environment.NewLine, errorMessage);
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
