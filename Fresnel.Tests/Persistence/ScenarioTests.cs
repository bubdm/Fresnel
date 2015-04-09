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

        [Test]
        public void ShouldAddOrderItemToOrder()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

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
}