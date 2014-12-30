using Castle.DynamicProxy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Tests.Proxies.DynamicProxy
{


    public class ProxyFactory
    {
        private ProxyGenerator _ProxyGenerator = new ProxyGenerator();

        private Type[] _Interfaces = new Type[] { typeof(IProxy) };

        public T BuildFor<T>(T targetObject, Type realType)
            where T : class
        {
            var interceptorA = new MockInterceptorA();
            var interceptorB = new MockInterceptorB();

            var innerProxy = _ProxyGenerator.CreateClassProxyWithTarget(realType, _Interfaces, targetObject, interceptorB);

            var outerProxy = _ProxyGenerator.CreateClassProxyWithTarget(realType, _Interfaces, innerProxy, interceptorA);

            return (T)outerProxy;
        }

    }


}

