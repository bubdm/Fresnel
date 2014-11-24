using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;
using System.Linq;

namespace Envivo.Fresnel.Proxies
{

    public class CollectionAddInterceptor : IInterceptor, IDisposable
    {
        public CollectionAddInterceptor()
        {
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(invocation.ToString());
            var oCollection = (CollectionObserver)((IFresnelProxy)invocation.Proxy).Meta;

            var item = this.ExtractItemToAddFrom(invocation);
            if (item != null &&
                item.GetType().IsNonReference() == false)
            {
                var oItem = (ObjectObserver)((IFresnelProxy)this.ProxyCache.GetProxy(item)).Meta;
                var oCollectionProp = this.DetermineOuterProperty(oCollection);

                this.PreInvoke(invocation, oCollection, oCollectionProp, oItem);
                invocation.Proceed();

                var oAddedItem = oItem;
                if (invocation.ReturnValue != null &&
                    invocation.GetType().IsNonReference() == false)
                {
                    // It's possible that the Add() method returns a different object:
                    oAddedItem = (ObjectObserver)((IFresnelProxy)this.ProxyCache.GetProxy(invocation.ReturnValue)).Meta;
                    //oAddedItem.IsReflectionEnabled = false;
                }

                this.PostInvoke(oCollection, oCollectionProp, oAddedItem);
            }
            else
            {
                // In case we don't recognise the operation:
                invocation.Proceed();
            }
        }

        private object ExtractItemToAddFrom(IInvocation invocation)
        {
            if (invocation.Method.Name == "Add" && invocation.Arguments.Length == 1)
            {
                return invocation.Arguments[0];
            }
            else if (invocation.Method.Name == "InsertItem" && invocation.Arguments.Length == 2)
            {
                return invocation.Arguments[1];
            }

            return null;
        }

        private BasePropertyObserver DetermineOuterProperty(CollectionObserver oCollection)
        {
            switch (oCollection.OuterProperties.Count())
            {
                case 0:
                    return null;

                case 1:
                    return oCollection.OuterProperties.First();

                default:
                    // Return the most recently accessed Property:
                    return oCollection.OuterProperties.OrderBy(p => p.LastAccessedAtUtc).Last();
            }
        }

        private void PreInvoke(IInvocation invocation, CollectionObserver oCollection, BasePropertyObserver oCollectionProp, ObjectObserver oItemToAdd)
        {

            //var addCheck = oCollectionProp.Permissions.Add.Check(oItemToAdd);
            //if (addCheck.Failed)
            //{
            //    oCollectionProp.ErrorMessage = addCheck.FailureReason;
            //    throw new FresnelException(addCheck.FailureReason);
            //}

            //if (oCollectionProp.PropertyTemplate.Attributes.Get<CollectionPropertyAttribute>().HasUniqueItems &&
            //    oCollection.Contains(oItemToAdd))
            //{
            //    var msg = "The list doesn't allow duplicate items";
            //    oCollectionProp.ErrorMessage = msg;
            //    throw new FresnelException(msg);
            //}

            //// Make the Persistor aware of the change that is about to happen:
            //oCollection.Persistor.WakeUp();

            ////-----

            //var isValueObject = oItemToAdd != null &&
            //                    oItemToAdd.IsNull == false &&
            //                    oItemToAdd.IsValueObject;
            //if (isValueObject)
            //{
            //    // ValueObjects are cloned:
            //    invocation.SetArgumentValue(0, ((ObjectObserver)oItemToAdd).Clone().RealObject);
            //}
        }

        private void PostInvoke(CollectionObserver oCollection, BasePropertyObserver oCollectionProp, ObjectObserver oAddedItem)
        {
            //oCollection.Persistor.Suspend();

            //// TODO: Do we need to check if the Add/Insert method returned an object that the Collection doesn't accept?
            ////       (See CollectionObserver.Add() )

            ////-----

            //if (oAddedItem != null)
            //{
            //    // Make the item aware that it is associated with this Collection:
            //    oCollection.AssociateWith(oAddedItem);

            //    // NB: Unsaved Domain Objects should NOT affect the dirty status:
            //    if (oAddedItem.IsPersistable)
            //    {
            //        oAddedItem.ChangeTracker.IsMarkedForAddition = true;
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
