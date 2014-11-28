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
        private readonly string[] _ObjectMemberNames = new string[] { "Equals", "ToString", "GetType", "GetHashCode", "Dispose", "Finalize", "Error" };

        private IInterceptor[] _EmptyInterceptors = new IInterceptor[0];

        public bool CanHandle(MethodInfo method)
        {
            return _ObjectMemberNames.Contains(method.Name);
        }

        public IEnumerable<IInterceptor> GetInterceptors(IEnumerable<IInterceptor> allInterceptors)
        {
            return _EmptyInterceptors;
        }

    }

}
