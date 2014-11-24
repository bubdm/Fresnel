using Castle.DynamicProxy;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies
{

    public class NotifyCollectionChangedInterceptor : IInterceptor
    {
        public NotifyCollectionChangedInterceptor()
        {
        }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(invocation.ToString());
            NotifyCollectionChangedEventHandler eventHandler = null;

            if (invocation.Method.DeclaringType.Equals(typeof(INotifyCollectionChanged)))
            {
                if (invocation.Method.Name == "add_CollectionChanged")
                {
                    eventHandler += invocation.Arguments[0] as NotifyCollectionChangedEventHandler;
                }
                else if (invocation.Method.Name == "remove_CollectionChanged")
                {
                    eventHandler -= invocation.Arguments[0] as NotifyCollectionChangedEventHandler;
                }
            }
            else if (invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("set_"))
            {
                this.RaiseEvent(invocation, eventHandler);
            }

            if (invocation.InvocationTarget != null)
            {
                invocation.Proceed();
            }
        }

        private void RaiseEvent(IInvocation invocation, NotifyCollectionChangedEventHandler eventHandler)
        {
            // NB: This is to prevent failures if _EventHandler becomes null:
            if (eventHandler == null)
                return;

            var eventArgs = this.CreateCollectionChangedEventArgs(invocation);
            if (eventArgs == null)
                return;

            eventHandler.Invoke(invocation.Proxy, eventArgs);
        }

        private NotifyCollectionChangedEventArgs CreateCollectionChangedEventArgs(IInvocation invocation)
        {
            object item = null;
            NotifyCollectionChangedAction? action = null;
            if (invocation.Method.Name == "InsertItem" && invocation.Arguments.Length == 2)
            {
                item = invocation.Arguments[1];
                action = NotifyCollectionChangedAction.Add;
            }
            else if (invocation.Method.Name == "Add" && invocation.Arguments.Length == 1)
            {
                item = invocation.Arguments[0];
                action = NotifyCollectionChangedAction.Add;
            }
            else if (invocation.Method.Name == "RemoveItem" && invocation.Arguments.Length == 2)
            {
                item = invocation.Arguments[1];
                action = NotifyCollectionChangedAction.Remove;
            }
            else if (invocation.Method.Name == "Remove" && invocation.Arguments.Length == 1)
            {
                item = invocation.Arguments[0];
                action = NotifyCollectionChangedAction.Remove;
            }
            else if (invocation.Method.Name == "RemoveAt" && invocation.Arguments.Length == 1)
            {
                // TODO: Extract the item from the list:
                item = invocation.Arguments[0];
                action = NotifyCollectionChangedAction.Remove;
            }
            else if (invocation.Method.Name == "SetItem" && invocation.Arguments.Length == 1)
            {
                // TODO: Extract the item from the list:
                item = invocation.Arguments[0];
                action = NotifyCollectionChangedAction.Replace;
            }
            else if (invocation.Method.Name == "Clear")
            {
                action = NotifyCollectionChangedAction.Reset;
            }

            return action == null ?
                             null :
                             new NotifyCollectionChangedEventArgs(action.Value, item);
        }


        public override bool Equals(object obj)
        {
            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

    }

}
