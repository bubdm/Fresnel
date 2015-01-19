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
using Envivo.Fresnel.Core.Observers;
using System.Reflection;
using System.Collections.Generic;
using Envivo.Fresnel.DomainTypes;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class CollectionObserverTests
    {
        [Test()]
        public void ShouldIdentifyCollectionProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var poco = new Collection<SampleModel.Objects.PocoObject>();

            // Act:
            var observer = (CollectionObserver)observerBuilder.BuildFor(poco, poco.GetType());

            // Assert:
            Assert.AreNotEqual(0, observer.Template.Properties.Count());
        }

    }
}

