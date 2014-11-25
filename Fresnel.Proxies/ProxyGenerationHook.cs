using Castle.DynamicProxy;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Utils;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Envivo.Fresnel.Proxies
{

    public class ProxyGenerationHook : IProxyGenerationHook
    {
        private Type _ProxyType = typeof(IFresnelProxy);

        public void MethodsInspected()
        {
            // We're not interested in notifications
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            // We're not interested in notifications
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            var msg = string.Concat("Inspecting ", type.Name, ".", methodInfo.Name);
            Debug.WriteLine(msg);
            
            if (_ProxyType == type)
                return false;

            if (_ProxyType.IsDerivedFrom(type))
                return false;

            return true;
        }
    }
}
