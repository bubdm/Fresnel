using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using NUnit.Framework;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class NamespaceHierarchyBuilderTests
    {
        [Test]
        public void ShouldCreateHierarchy()
        {
            // Arrange:
            var container = new ContainerFactory().Build();

            var hierarchyBuilder = container.Resolve<NamespaceHierarchyBuilder>();

            // Act:
            var hierarchy = hierarchyBuilder.BuildTreeFor(typeof(Product).Assembly);

            // Assert:
            Assert.IsNotNull(hierarchy);

            var superClassNode = hierarchy.FindNodeByName(typeof(Role).Name);
            Assert.AreNotEqual(0, superClassNode.Children.Count());

            Assert.IsTrue(superClassNode.Children.All(c => c.IsSubClass));
        }
    }
}