﻿using Autofac;
using Envivo.Fresnel.CompositionRoot;
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
                PageSize = 100,
                PageNumber = 1
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
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = controller.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());

            // All nulls should appear after the text values:
            var textValues = searchResponse.Result.Items
                                    .Select(i => i.Properties.Single(p => p.InternalName == "NormalText").State.Value)
                                    .Cast<string>()
                                    .ToList();

            var indexOfFirstNull = textValues.IndexOf(null);
            var nonNullValues = textValues.GetRange(0, indexOfFirstNull);

            for (var i = 1; i < nonNullValues.Count; i++)
            {
                var previousValue = nonNullValues[i - 1];
                var currentValue = nonNullValues[i];
                Assert.LessOrEqual(previousValue, currentValue);
            }
        }

        [Test]
        public void ShouldSearchForObjectsInDecendingOrder()
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
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = controller.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());

            // All nulls should appear after the text values:
            var textValues = searchResponse.Result.Items
                                    .Select(i => i.Properties.Single(p => p.InternalName == "NormalText").State.Value)
                                    .Cast<string>()
                                    .ToList();

            var indexOfFirstNull = textValues.IndexOf(null);
            var nonNullValues = textValues.GetRange(0, indexOfFirstNull);

            for (var i = 1; i < nonNullValues.Count; i++)
            {
                var previousValue = nonNullValues[i - 1];
                var currentValue = nonNullValues[i];
                Assert.GreaterOrEqual(previousValue, currentValue);
            }
        }


        [Test]
        public void ShouldPreventOrderingByCollections()
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
                OrderBy = "ChildObjects",
                IsDescendingOrder = false,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = controller.SearchObjects(searchRequest);

            // Assert:
            Assert.IsFalse(searchResponse.Passed);
            Assert.IsNull(searchResponse.Result);
        }


        [Test]
        public void ShouldSeachForObjectPropertyObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var createResponse = toolboxController.Create(typeof(Category).FullName);

            var searchRequest = new SearchPropertyRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                PropertyName = "Money",
                OrderBy = "",
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = explorerController.SearchPropertyObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
            Assert.IsTrue(searchResponse.Result.Items.All(i => i.Type == typeof(Money).Name));
        }

        [Test]
        public void ShouldSeachForCollectionPropertyObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var createResponse = toolboxController.Create(typeof(PocoObject).FullName);

            // Act:
            var searchRequest = new SearchPropertyRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                PropertyName = "ChildObjects",
                OrderBy = "",
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = explorerController.SearchPropertyObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
            Assert.IsTrue(searchResponse.Result.Items.All(i => i.Type == typeof(PocoObject).Name));
        }


        [Test]
        public void ShouldSeachForObjectParameterObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var classType = typeof(Fresnel.SampleModel.MethodTests);
            var createResponse = toolboxController.Create(classType.FullName);

            var searchRequest = new SearchParameterRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                MethodName = "MethodWithObjectParameters",
                ParameterName = "category",
                OrderBy = "",
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = explorerController.SearchParameterObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
            Assert.IsTrue(searchResponse.Result.Items.All(i => i.Type == typeof(Category).Name));
        }

        [Test]
        public void ShouldSeachForCollectionParameterObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var classType = typeof(Fresnel.SampleModel.MethodTests);
            var createResponse = toolboxController.Create(classType.FullName);

            var searchRequest = new SearchParameterRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                MethodName = "MethodWithObjectParameters",
                ParameterName = "pocos",
                OrderBy = "",
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = explorerController.SearchParameterObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
            Assert.IsTrue(searchResponse.Result.Items.All(i => i.Type == typeof(PocoObject).Name));
        }

    }

}