
using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Diagnostics;
using System.Linq;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class CollectionRemoveInterceptor : IInterceptor, IDisposable
    {
        public CollectionRemoveInterceptor()
        {
        }

        public ProxyCache ProxyCache { get; set; }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name);
            var oCollection = (CollectionObserver)((IFresnelProxy)invocation.Proxy).Meta;

            var item = this.ExtractItemToRemoveFrom(invocation);
            if (item != null &&
                item.GetType().IsNonReference() == false)
            {
                var oItem = (ObjectObserver)((IFresnelProxy)this.ProxyCache.GetProxy(item)).Meta;
                var oCollectionProp = this.DetermineOuterProperty(oCollection);

                this.PreInvoke(oCollection, oCollectionProp, oItem);

                invocation.Proceed();

                this.PostInvoke(oCollection, oCollectionProp, oItem);
            }
            else
            {
                // In case we don't recognise the operation:
                invocation.Proceed();
            }
        }

        private object ExtractItemToRemoveFrom(IInvocation invocation)
        {
            if (invocation.Method.Name == "Remove" && invocation.Arguments.Length == 1)
            {
                return invocation.Arguments[0];
            }
            else if (invocation.Method.Name == "RemoveAt" && invocation.Arguments.Length == 1)
            {
                // TODO: Figure out which item is at the index:
                return invocation.Arguments[1];
            }
            else if (invocation.Method.Name == "RemoveItem" && invocation.Arguments.Length == 1)
            {
                // TODO: Figure out which item is at the index:
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

        private void PreInvoke(CollectionObserver oCollection, BasePropertyObserver oCollectionProp, ObjectObserver oItemToRemove)
        {
            //// Check if the operation is valid for the "Aggregate Root" contraint:
            ////if (My.Application.UnitOfWorkManager.IsPartOfAggregateRoot(My.Application.UI.ActiveWidget) == false)
            ////{
            ////    string msg = string.Format("'{0}.{1}' cannot be modified here. Please find it's owner and modify it there.", _oCollectionProp.OuterObject.FriendlyName, _oCollectionProp.FriendlyName);
            ////    return Assertion.Fail(msg);
            ////}

            //if (oCollection != null &&
            //    oCollection.Template.ContainsRemoveFor(oItemToRemove.RealObjectType) == false)
            //{
            //    throw new FresnelException("The list doesn't have a method to Remove this item");
            //}

            //if (oCollectionProp != null)
            //{
            //    var removeCheck = oCollectionProp.Permissions.Remove.Check();
            //    if (removeCheck.Failed)
            //    {
            //        oCollectionProp.ErrorMessage = removeCheck.FailureReason;
            //        throw new FresnelException(removeCheck.FailureReason);
            //    }
            //}

            //// Make the Persistor aware of the change that is about to happen:
            //oCollection.Persistor.WakeUp();
        }

        private void PostInvoke(CollectionObserver oCollection, BasePropertyObserver oCollectionProp, ObjectObserver oRemovedItem)
        {
            //oCollection.Persistor.Suspend();

            //// NB: Unsaved Domain Objects should NOT affect the dirty status:
            //this.UpdateChangeTrackers(oCollection, oRemovedItem);

            //// We can only disassociate after the Persistence settings have been modified:
            //oCollection.DisassociateFrom(oRemovedItem);
        }

        // Copied from CollectionObserver
        private void UpdateChangeTrackers(CollectionObserver oCollection, ObjectObserver oRemovedItem)
        {
            var tRemovedItem = oRemovedItem.Template;

            if (tRemovedItem.IsPersistable == false)
                return;

            //oRemovedItem.ChangeTracker.IsMarkedForRemoval = true;

            //if (oRemovedItem.ChangeTracker.IsNewInstance)
            //{
            //    // This prevents unsaved instances remaining linked to this collection:
            //    oCollection.ChangeTracker.RemoveFromDirtyObjectGraph(oRemovedItem);
            //    oCollection.ChangeTracker.DirtyChildren.Remove(oRemovedItem);
            //}
            //else
            //{
            //    oRemovedItem.ChangeTracker.MarkForRemovalFrom(oCollection);
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
