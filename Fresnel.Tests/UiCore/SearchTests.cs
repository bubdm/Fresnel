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
    public class SearchTests
    {

        [Test]
        public void ShouldSearchForObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(PocoObject).FullName,
                PageSize = 100
            };

            var searchResponse = controller.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);

            // We should have the results that we asked for:
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
            Assert.IsTrue(searchResponse.Result.Items.Count() <= searchRequest.PageSize);

            // The Results should show all Properties for the items:
            Assert.AreEqual(10, searchResponse.Result.ElementProperties.Count());
        }

        [Test]
        public void ShouldSearchForObjectsInAscendingOrder()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(PocoObject).FullName,
                OrderBy = "NormalText",
                IsDescendingOrder = false,
                PageSize = 100
            };

            var searchResponse = controller.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
        }

    }
}