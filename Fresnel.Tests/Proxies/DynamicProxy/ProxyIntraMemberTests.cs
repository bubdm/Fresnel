using NUnit.Framework;
using Autofac;
using System;
using System.Linq;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using System.Reflection;
using System.Collections.Generic;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.Proxies;
using System.ComponentModel;
using Envivo.Fresnel.DomainTypes;
using System.Diagnostics;

namespace Envivo.Fresnel.Tests.Proxies.DynamicProxy
{
    //[TestFixture, Ignore("Disabled as only used for experimenting with DynamicProxy")]
    public class ProxyIntraMemberTests
    {

        [Test()]
        public void ShouldInterceptIntraPropertyAccess()
        {
            // Arrange:
            var realObject = new A();
            var proxy = new ProxyFactory().BuildFor(realObject, typeof(A));

            // Act:
            var result = proxy.Value;

            // Assert:

        }


    }



}

