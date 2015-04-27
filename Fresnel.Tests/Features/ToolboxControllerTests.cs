using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Fresnel.SampleModel.Persistence;
using Fresnel.Tests;
using NUnit.Framework;
using System.Linq;

namespace Envivo.Fresnel.Tests.Features
{
    [TestFixture()]
    public class ToolboxControllerTests
    {
        private TestScopeContainer _TestScopeContainer = null;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _TestScopeContainer = new TestScopeContainer(new CustomDependencyModule());

            using (var scope = _TestScopeContainer.BeginScope())
            {
                var engine = _TestScopeContainer.Resolve<Core.Engine>();
                engine.RegisterDomainAssembly(typeof(MultiType).Assembly);
            }
        }

        [Test]
        public void ShouldReturnToolboxClassItems()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var controller = _TestScopeContainer.Resolve<ToolboxController>();

                // Act:
                var results = controller.GetClassHierarchy();

                // Assert:
                Assert.AreNotEqual(0, results.Count());
            }
        }

        [Test]
        public void ShouldCreateNewInstance()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var controller = _TestScopeContainer.Resolve<ToolboxController>();

                // Act:
                var createRequest = new CreateObjectRequest()
                {
                    ClassTypeName = typeof(ObjectWithCtorInjection).FullName
                };
                var response = controller.Create(createRequest);

                // Assert:
                Assert.IsTrue(response.Passed);
                Assert.IsInstanceOf<ObjectVM>(response.NewObject);
            }
        }

        [Test]
        public void ShouldSearchForObjects()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var controller = _TestScopeContainer.Resolve<ToolboxController>();

                // Act:
                var searchRequest = new SearchObjectsRequest()
                {
                    SearchType = typeof(Product).FullName,
                    PageSize = 10,
                    PageNumber = 1,
                };

                var searchResponse = controller.SearchObjects(searchRequest);

                // Assert:
                Assert.IsTrue(searchResponse.Passed);

                // We should have the results that we asked for:
                Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
                Assert.IsTrue(searchResponse.Result.Items.Count() <= searchRequest.PageSize);

                // The Results should show all Properties for the items:
                Assert.AreEqual(5, searchResponse.Result.ElementProperties.Count());
            }
        }
    }
}