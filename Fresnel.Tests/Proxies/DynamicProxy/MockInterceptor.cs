using Castle.DynamicProxy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Tests.Proxies.DynamicProxy
{


    public class MockInterceptorA : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine("Before " + invocation.ToString());

            invocation.ReturnValue = invocation.Method.Invoke(invocation.InvocationTarget, invocation.Arguments);

            Debug.WriteLine("After " + invocation.ToString());
        }
    }


    public class MockInterceptorB : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine("Before " + invocation.ToString());

            invocation.Proceed();

            Debug.WriteLine("After " + invocation.ToString());
        }
    }

}

