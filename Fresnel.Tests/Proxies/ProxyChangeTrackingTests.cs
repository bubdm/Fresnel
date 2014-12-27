//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
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

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ProxyChangeTrackingTests
    {
        [Test]
        public void ShouldDetectPropertyChanges()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Act:
            pocoProxy.NormalText = DateTime.Now.ToString();
            pocoProxy.NormalDate = DateTime.Now;

            // Assert:
            var state = pocoProxy as IProxyState;
            // NB: This also includes properties modified in the object's ctor:
            Assert.Greater(state.SessionJournal.PropertyChanges.Count, 0);
        }

        [Test]
        public void ShouldDetectCollectionChanges()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Act:
            // This should intercept the property getters used within the method:
            pocoProxy.ChildObjects.Add(new SampleModel.Objects.PocoObject());
            pocoProxy.ChildObjects.Add(new SampleModel.Objects.PocoObject());

            // Assert:
            var state = pocoProxy as IProxyState;
            Assert.AreEqual(2, state.SessionJournal.CollectionAdditions.Count);
        }

        [Test]
        public void ShouldDetectIntraMemberCalls()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var proxyCache = container.Resolve<ProxyCache>();

            var poco = new SampleModel.Objects.PocoObject();
            var pocoProxy = proxyCache.GetProxy(poco);

            // Act:
            // This should intercept the actions used within the method:
            pocoProxy.AddSomeChildObjects();

            // Assert:
            var state = pocoProxy as IProxyState;
            Assert.AreEqual(3, state.SessionJournal.CollectionAdditions.Count);
        }
    }

}

