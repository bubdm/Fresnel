using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.Northwind;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class CollectionObserverTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldIdentifyCollectionProperties()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var observerBuilder = container.Resolve<AbstractObserverBuilder>();

            var collection = new Collection<Product>();

            // Act:
            var observer = (CollectionObserver)observerBuilder.BuildFor(collection, collection.GetType());

            // Assert:
            Assert.AreNotEqual(0, observer.Template.Properties.Count());
        }
    }
}