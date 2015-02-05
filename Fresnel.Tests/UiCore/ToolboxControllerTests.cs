using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using NUnit.Framework;
using System.Linq;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class ToolboxControllerTests
    {
        [Test]
        public void ShouldReturnToolboxClassItems()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var controller = container.Resolve<ToolboxController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            // Act:
            var results = controller.GetClassHierarchy();

            // Assert:
            Assert.AreNotEqual(0, results.Count());
        }

        [Test]
        public void ShouldCreateNewInstance()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var controller = container.Resolve<ToolboxController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.Objects.DependencyAwareObject).Assembly);

            // Act:
            var response = controller.Create("Envivo.Fresnel.SampleModel.Objects.DependencyAwareObject");

            // Assert:
            Assert.IsTrue(response.Passed);
            Assert.IsInstanceOf<ObjectVM>(response.NewObject);
        }
    }
}