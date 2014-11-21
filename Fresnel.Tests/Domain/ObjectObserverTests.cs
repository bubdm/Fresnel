//  Author:
//       Vijay Patel
//
// Copyright (c) 2014 Vijay Patel
//
using NUnit.Framework;
using Autofac;
using System;
using System.Linq;
using Envivo.Fresnel;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Engine.Observers;
using System.Reflection;
using System.Collections.Generic;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ObjectObserverTests
    {
        [Test()]
        public void ShouldCreateObjectObserver()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            // Act:
            var observer = observerBuilder.BuildFor(poco, poco.GetType());

            // Assert:
            Assert.IsNotNull(observer);
        }

        [Test()]
        public void ShouldGetObjectObserverFromCache()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var poco = new SampleModel.Objects.PocoObject();

            var observerCache = container.Resolve<ObserverCache>();

            // Act:
            var observer = observerCache.GetObserver(poco);

            // Assert:
            Assert.IsNotNull(poco);
        }

        [Test()]
        public void ShouldCreatePropertyObservers()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            // Act:
            var observer = (ObjectObserver)observerBuilder.BuildFor(poco, poco.GetType());

            // Assert:
            Assert.AreNotEqual(0, observer.Properties.Count());

            foreach (var prop in observer.Properties.Values)
            {
                Assert.IsNotNull(prop.FullName);
            }
        }

        [Test()]
        public void ShouldCreateMethodObservers()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();

            // Act:
            var observer = (ObjectObserver)observerBuilder.BuildFor(poco, poco.GetType());

            // Assert:
            Assert.AreNotEqual(0, observer.Methods.Count());

            foreach (var method in observer.Methods.Values)
            {
                var parameters = method.Parameters;
                Assert.IsNotNull(method.Parameters);

                foreach (var p in parameters.Values)
                {
                    Assert.IsNotNull(p.FullName);
                }
            }
        }

        [Test()]
        public void ShouldCreateCollectionObserver()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var poco = new SampleModel.Objects.PocoObject();
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();

            // Act:
            var observer = observerBuilder.BuildFor(poco.ChildObjects, poco.ChildObjects.GetType());

            // Assert:
            Assert.IsNotNull(observer);
        }

    }
}

