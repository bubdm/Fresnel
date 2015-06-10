using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using Fresnel.SampleModel.Persistence;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Persistence
{
    [TestFixture()]
    public class ScenarioTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
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
        public void ShouldAddOrderItemToNewOrder()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var toolboxController = _TestScopeContainer.Resolve<ToolboxController>();
                var explorerController = _TestScopeContainer.Resolve<ExplorerController>();
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();

                // Act:
                var createRequest = new CreateObjectRequest()
                {
                    ClassTypeName = typeof(Order).FullName
                };
                var createResponse = toolboxController.Create(createRequest);
                var orderId = createResponse.NewObject.ID;

                // This ensures the Collection can be tracked:
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = orderId,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems)
                };
                var getResult = explorerController.GetObjectProperty(getRequest);

                // Add a new OrderItem:
                var orderItemType = typeof(OrderItem);
                var collectionAddNewRequest = new CollectionAddNewRequest()
                {
                    ParentObjectID = orderId,
                    CollectionPropertyName = getRequest.PropertyName,
                    ElementTypeName = orderItemType.FullName
                };
                var collectionAddResponse = explorerController.AddNewItemToCollection(collectionAddNewRequest);

                // Save everything:
                var saveRequest = new SaveChangesRequest()
                {
                    ObjectID = orderId
                };
                var saveResponse = explorerController.SaveChanges(saveRequest);

                // Assert:
                Assert.IsTrue(saveResponse.Passed);
            }
        }

        [Test]
        public void ShouldAddOrderItemToExistingOrder()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var toolboxController = _TestScopeContainer.Resolve<ToolboxController>();
                var explorerController = _TestScopeContainer.Resolve<ExplorerController>();
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();

                // Act:
                var searchRequest = new SearchObjectsRequest()
                {
                    SearchType = typeof(Order).FullName,
                    PageSize = 100,
                    PageNumber = 1
                };
                var searchResponse = toolboxController.SearchObjects(searchRequest);
                var order = searchResponse.Result.Items.First();

                // This ensures the Collection can be tracked:
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems)
                };
                var getResult = explorerController.GetObjectProperty(getRequest);

                // Add a new OrderItem:
                var orderItemType = typeof(OrderItem);
                var collectionAddNewRequest = new CollectionAddNewRequest()
                {
                    ParentObjectID = order.ID,
                    CollectionPropertyName = getRequest.PropertyName,
                    ElementTypeName = orderItemType.FullName
                };
                var collectionAddResponse = explorerController.AddNewItemToCollection(collectionAddNewRequest);
                var orderItem = collectionAddResponse.Modifications.NewObjects.First();

                // Save everything:
                var saveRequest = new SaveChangesRequest()
                {
                    ObjectID = orderItem.ID
                };
                var saveResponse = explorerController.SaveChanges(saveRequest);

                // Assert:
                Assert.IsTrue(saveResponse.Passed);
            }
        }
    }
}