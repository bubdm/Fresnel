using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.Utils;
using Fresnel.SampleModel.Persistence;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class SessionControllerTests
    {
        [Test]
        public void ShouldReturnSingleSession()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var controller = container.Resolve<SessionController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var now = DateTime.Now;

            // Act:
            var session1 = controller.GetSession();
            Thread.Sleep(100);

            var session2 = controller.GetSession();

            // Assert:
            Assert.AreEqual(Environment.UserName, session1.UserName);
            Assert.GreaterOrEqual(session1.LogonTime, now);
            Assert.AreEqual(session1.LogonTime, session2.LogonTime);
        }

        [Test]
        public void ShouldSaveAllChanges()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var sessionController = container.Resolve<SessionController>();
            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            // Act:
            // Start a new session:
            var session = sessionController.GetSession();

            var type = typeof(MultiType);
            var createResponse = toolboxController.Create(type.FullName);

            // Make some changes
            var request = new SetPropertyRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_String),
                NonReferenceValue = DateTime.Now.ToString()
            };
            var setResult = explorerController.SetProperty(request);

            // This should revert all changes:
            var saveRequest = new SaveChangesRequest()
            {
                ObjectID = request.ObjectID,
            };
            var saveResponse = explorerController.SaveChanges(saveRequest);

            // Assert:
            Assert.IsTrue(saveResponse.Passed);

            var getRequest = new GetObjectRequest()
            {
                ObjectID = request.ObjectID
            };
            var getResponse = explorerController.GetObject(getRequest);
            Assert.IsNotNull(getResponse.ReturnValue);
        }

        [Test]
        public void ShouldReportFailedConsistencyChecks()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var sessionController = container.Resolve<SessionController>();
            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            // Act:
            // Start a new session:
            var session = sessionController.GetSession();

            // Try to save an inconsistent object:
            var type = typeof(Product);
            var createResponse = toolboxController.Create(type.FullName);

            var saveRequest = new SaveChangesRequest()
            {
                ObjectID = createResponse.NewObject.ID,
            };
            var saveResponse = explorerController.SaveChanges(saveRequest);

            // Assert:
            Assert.IsFalse(saveResponse.Passed);
            Assert.AreNotEqual(0, saveResponse.Messages.Length);
        }

        [Test]
        public void ShouldCleanupSession()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var sessionController = container.Resolve<SessionController>();
            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            // Act:
            // Start a new session:
            var session = sessionController.GetSession();

            // Find an object to edit:
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                PageSize = 10,
                PageNumber = 1,
            };
            var searchResponse = toolboxController.SearchObjects(searchRequest);
            var pocoA = searchResponse.Result.Items.First();

            // Make a change
            var request = new SetPropertyRequest()
            {
                ObjectID = pocoA.ID,
                PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_DateTime),
                NonReferenceValue = DateTime.Now.ToString()
            };
            var setResult = explorerController.SetProperty(request);

            // This should revert all changes:
            sessionController.CleanUp();

            // Assert:
            searchResponse = toolboxController.SearchObjects(searchRequest);
            var persistedItem = searchResponse.Result.Items.First();

            var propertyValue = persistedItem
                                .Properties
                                .Single(p => p.InternalName == request.PropertyName)
                                .IsNonReference;

            Assert.AreNotEqual(request.NonReferenceValue, propertyValue);
        }
    }
}