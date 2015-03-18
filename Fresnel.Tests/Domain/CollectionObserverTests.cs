using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using NUnit.Framework;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class CollectionObserverTests
    {
        [Test]
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