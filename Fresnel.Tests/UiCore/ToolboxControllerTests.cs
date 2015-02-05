using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Fresnel.SampleModel.Persistence;
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

        [Test]
        public void ShouldGetAllObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var getRequest = new GetObjectsRequest()
            {
                TypeName = typeof(PocoObject).FullName,
                Skip = 0,
                Take = 10,
            };

            var getResponse = controller.GetObjects(getRequest);

            // Assert:
            Assert.IsTrue(getResponse.Passed);
            Assert.AreNotEqual(0, getResponse.Results.Count());
            Assert.IsTrue(getResponse.Results.Count() <= getRequest.Take);
        }
    }
}