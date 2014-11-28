using Castle.DynamicProxy;
using System.ComponentModel;
using System.Diagnostics;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    /// <summary>
    /// Intecepts calls made to Domain Objects
    /// </summary>
    public class NotifyPropertyChangedInterceptor : IInterceptor
    {
        private PropertyChangedEventHandler _EventHandler = null;

        public NotifyPropertyChangedInterceptor()
        {
        }

        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(this.GetType().Name);

            if (invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("set_"))
            {
                this.RaiseEvent(invocation, _EventHandler);
            }
            else if (invocation.Method.DeclaringType.Equals(typeof(INotifyPropertyChanged)))
            {
                if (invocation.Method.Name == "add_PropertyChanged")
                {
                    _EventHandler += invocation.Arguments[0] as PropertyChangedEventHandler;
                }
                else if (invocation.Method.Name == "remove_PropertyChanged")
                {
                    _EventHandler -= invocation.Arguments[0] as PropertyChangedEventHandler;
                }
            }

            if (invocation.InvocationTarget != null)
            {
                invocation.Proceed();
            }
        }

        private void RaiseEvent(IInvocation invocation, PropertyChangedEventHandler eventHandler)
        {
            // NB: This is to prevent failures if _EventHandler becomes null:
            if (eventHandler == null)
                return;

            eventHandler.Invoke(invocation.Proxy, new PropertyChangedEventArgs(invocation.Method.Name.Remove(0, 4)));
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
