using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Proxies.Interceptors
{

    public class IgnoreMethodInterceptorsSelector : IInterceptorsSelector
    {
        private readonly string[] _NamesToIgnore;
        private IInterceptor[] _EmptyInterceptors = new IInterceptor[0];

        public IgnoreMethodInterceptorsSelector()
        {
            object obj = null;
            IDisposable disposable = null;
            IFresnelProxy fresnelProxy = null;
            IProxyState proxyState = null;

            _NamesToIgnore = new string[]
            {
                LambdaExtensions.NameOf(() => obj.Equals(null)),
                LambdaExtensions.NameOf(() => obj.ToString()),
                LambdaExtensions.NameOf(() => obj.GetType()),
                LambdaExtensions.NameOf(() => obj.GetHashCode()),
                LambdaExtensions.NameOf(() => obj.ToString()),

                LambdaExtensions.NameOf(() => disposable.Dispose()),
                LambdaExtensions.NameOf(() => fresnelProxy.Meta),
                LambdaExtensions.NameOf(() => proxyState.ChangeLog),
            };
        }

        public bool CanHandle(MethodInfo method)
        {
            var methodName = method.IsSpecialName ?
                                method.Name.Remove(0, 4) :
                                method.Name;

            return _NamesToIgnore.Contains(methodName);
        }

        public IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors)
        {
            return _EmptyInterceptors;
        }

    }

}
