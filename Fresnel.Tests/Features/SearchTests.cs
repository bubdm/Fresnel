using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.Tests.Persistence;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using Fresnel.SampleModel.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Features
{
    [TestFixture()]
    public class SearchTests
    {

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            new TestDataGenerator().Generate();
        }

        [Test]
        public void ShouldSearchForObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
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
            Assert.AreEqual(12, searchResponse.Result.ElementProperties.Count());
        }

        [Test]
        public void ShouldSearchForObjectsInAscendingOrder()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var propName = LambdaExtensions.NameOf<MultiType>(x => x.A_String);
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                OrderBy = propName,
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
                                    .Select(i => i.Properties.Single(p => p.InternalName == propName).State.Value)
                                    .Cast<string>()
                                    .ToList();

            var indexOfFirstNull = textValues.IndexOf(null);
            var nonNullValues = textValues.GetRange(0, indexOfFirstNull);

            for (var i = 1; i < nonNullValues.Count; i++)
            {
                var previousValue = nonNullValues[i - 1].ToLower();
                var currentValue = nonNullValues[i].ToLower();
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
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var propName = LambdaExtensions.NameOf<MultiType>(x => x.A_String);
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                OrderBy = propName,
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
                                    .Select(i => i.Properties.Single(p => p.InternalName == propName).State.Value)
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
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var propName = LambdaExtensions.NameOf<MultiType>(x => x.A_Collection);
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                OrderBy = propName,
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
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var createRequest = new CreateObjectRequest()
            {
                ClassTypeName = typeof(MultiType).FullName
            };
            var createResponse = toolboxController.Create(createRequest);

            var propName = LambdaExtensions.NameOf<MultiType>(x => x.An_Object);
            var searchRequest = new SearchPropertyRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                PropertyName = propName,
                OrderBy = "",
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = explorerController.SearchPropertyObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
            Assert.IsTrue(searchResponse.Result.Items.All(i => i.Type == typeof(TextValues).Name));
        }

        [Test]
        public void ShouldSeachForCollectionPropertyObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var createRequest = new CreateObjectRequest()
            {
                ClassTypeName = typeof(MultiType).FullName
            };
            var createResponse = toolboxController.Create(createRequest);

            // Act:
            var propName = LambdaExtensions.NameOf<MultiType>(x => x.A_Collection);
            var searchRequest = new SearchPropertyRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                PropertyName = propName,
                OrderBy = "",
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = explorerController.SearchPropertyObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
            Assert.IsTrue(searchResponse.Result.Items.All(i => i.Type == typeof(BooleanValues).Name));
        }

        [Test]
        public void ShouldSeachForMethodParameterObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var createRequest = new CreateObjectRequest()
            {
                ClassTypeName = typeof(MethodSamples).FullName
            };
            var createResponse = toolboxController.Create(createRequest);

            var methodName = LambdaExtensions.NameOf<MethodSamples>(x => x.MethodWithObjectParameters(null));
            var searchRequest = new SearchParameterRequest()
            {
                ObjectID = createResponse.NewObject.ID,
                MethodName = methodName,
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

        //[Test]
        //public void ShouldSeachForCollectionParameterObjects()
        //{
        //    // Arrange:
        //    var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
        //    var container = new ContainerFactory().Build(customDependencyModules);

        //    var engine = container.Resolve<Core.Engine>();
        //    engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

        //    var toolboxController = container.Resolve<ToolboxController>();
        //    var explorerController = container.Resolve<ExplorerController>();

        //    // Act:
        //    var classType = typeof(Fresnel.SampleModel.BasicTypes.MethodSamples);
        //    var createResponse = toolboxController.Create(classType.FullName);

        //    var searchRequest = new SearchParameterRequest()
        //    {
        //        ObjectID = createResponse.NewObject.ID,
        //        MethodName = "MethodWithObjectParameters",
        //        ParameterName = "pocos",
        //        OrderBy = "",
        //        IsDescendingOrder = true,
        //        PageSize = 100,
        //        PageNumber = 1
        //    };

        //    var searchResponse = explorerController.SearchParameterObjects(searchRequest);

        //    // Assert:
        //    Assert.IsTrue(searchResponse.Passed);
        //    Assert.AreNotEqual(0, searchResponse.Result.Items.Count());
        //    Assert.IsTrue(searchResponse.Result.Items.All(i => i.Type == typeof(Product).Name));
        //}

        [Test]
        public void ShouldShowAllHeadersForClassHierarchy()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(BaseParty).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(BaseParty).FullName,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = controller.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.GreaterOrEqual(searchResponse.Result.ElementProperties.Count(), 3);
        }

        [Test]
        public void ShouldAssignCorrectValuesForClassHierarchy()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(BaseParty).Assembly);

            var controller = container.Resolve<ToolboxController>();

            // Act:
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(BaseParty).FullName,
                PageSize = 100,
                PageNumber = 1
            };

            var searchResponse = controller.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);

            var templateCache = container.Resolve<TemplateCache>();
            var tPerson = (ClassTemplate)templateCache.GetTemplate<Person>();
            var tOrganisation = (ClassTemplate)templateCache.GetTemplate<Organisation>();

            // All "Person" rows should have blanks for "Organisation" columns
            CheckPropertyColumnsDoNotCollide(searchResponse.Result, tPerson, tOrganisation);

            // All "Organisation" rows should have blanks for "Person" columns
            CheckPropertyColumnsDoNotCollide(searchResponse.Result, tOrganisation, tPerson);
        }

        private void CheckPropertyColumnsDoNotCollide(SearchResultsVM searchResult, ClassTemplate tClassToCheck, ClassTemplate tOtherClass)
        {
            var rowsToCheck = searchResult.Items.Where(i => i.Name == tClassToCheck.Name).ToArray();

            foreach (var row in rowsToCheck)
            {
                var visibleProperties = tOtherClass.Properties.VisibleOnly;

                foreach (var tProp in visibleProperties)
                {
                    var propVM = row.Properties.Single(p => p.InternalName == tProp.Name);
                    Assert.IsNull(propVM.State.Value);
                }
            }
        }

        [Test]
        public void ShouldFilterTextProperties()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var filterPropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_String);
            var filterValue = "a";
            var searchFilters = new List<SearchFilter>()
            {
                new SearchFilter() { PropertyName = filterPropertyName, FilterValue = filterValue }
            };

            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                SearchFilters = searchFilters.ToArray(),
                PageSize = 100,
                PageNumber = 1
            };
            var searchResponse = toolboxController.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());

            var columnValues = searchResponse.Result.Items
                            .Select(i => i.Properties.Single(p => p.InternalName.IsSameAs(filterPropertyName)).State.Value)
                            .Where(t => t != null)
                            .Cast<string>()
                            .Select(t => t.ToLower())
                            .ToList();

            Assert.IsTrue(columnValues.All(t => t.Contains(filterValue.ToLower())));
        }

        [Test]
        public void ShouldFilterBooleanProperties()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var filterPropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_Boolean);
            var filterValue = true;
            var searchFilters = new List<SearchFilter>()
            {
                new SearchFilter() { PropertyName = filterPropertyName, FilterValue = filterValue }
            };

            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                SearchFilters = searchFilters.ToArray(),
                PageSize = 100,
                PageNumber = 1
            };
            var searchResponse = toolboxController.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());

            var columnValues = searchResponse.Result.Items
                            .Select(i => i.Properties.Single(p => p.InternalName.IsSameAs(filterPropertyName)).State.Value)
                            .Where(t => t != null)
                            .Cast<bool>()
                            .ToList();

            Assert.IsTrue(columnValues.All(t => t == filterValue));
        }

        [Test]
        public void ShouldFilterDateTimeProperties()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            var persistenceService = container.Resolve<IPersistenceService>();
            var allKnownDates = persistenceService
                                    .GetObjects<MultiType>()
                                    .Select(mt => mt.A_DateTime)
                                    .ToArray();
            var mostFrequentDate = allKnownDates
                                    .Select(dt => dt.Date)
                                    .GroupBy(d => d)
                                    .OrderBy(g => g.Count())
                                    .Last()
                                    .Key;

            // Act:
            var filterPropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_DateTime);
            var filterValue = mostFrequentDate;
            var searchFilters = new List<SearchFilter>()
            {
                new SearchFilter() { PropertyName = filterPropertyName, FilterValue = filterValue }
            };

            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                SearchFilters = searchFilters.ToArray(),
                PageSize = 100,
                PageNumber = 1
            };
            var searchResponse = toolboxController.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());

            var columnValues = searchResponse.Result.Items
                            .Select(i => i.Properties.Single(p => p.InternalName.IsSameAs(filterPropertyName)).State.Value)
                            .Where(v => v != null)
                            .Select(v => DateTime.Parse(v.ToStringOrNull()))
                            .Cast<DateTime>()
                            .ToList();

            Assert.IsTrue(columnValues.All(t => t.Date == filterValue.Date));
        }

        [Test]
        public void ShouldFilterEnumProperties()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MultiType).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            // Act:
            var filterPropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_Bitwise_Enum);
            var filterValue = CombinationOptions.Cheese;
            var searchFilters = new List<SearchFilter>()
            {
                new SearchFilter() { PropertyName = filterPropertyName, FilterValue = filterValue }
            };

            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(MultiType).FullName,
                SearchFilters = searchFilters.ToArray(),
                PageSize = 100,
                PageNumber = 1
            };
            var searchResponse = toolboxController.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Passed);
            Assert.AreNotEqual(0, searchResponse.Result.Items.Count());

            var columnValues = searchResponse.Result.Items
                            .Select(i => i.Properties.Single(p => p.InternalName.IsSameAs(filterPropertyName)).State.Value)
                            .Select(v => Enum.Parse(typeof(CombinationOptions), v.ToStringOrNull()))
                            .Cast<CombinationOptions>()
                            .ToList();

            Assert.IsTrue(columnValues.All(v => (v & filterValue) != 0));
        }

        [Test]
        public void ShouldNotAllowSortingBySubclassProperty()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(BaseParty).Assembly);

            var toolboxController = container.Resolve<ToolboxController>();

            // Act:
            var propName = LambdaExtensions.NameOf<Person>(x => x.FirstName);
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(BaseParty).FullName,
                OrderBy = propName,
                PageSize = 100,
                PageNumber = 1
            };
            var searchResponse = toolboxController.SearchObjects(searchRequest);

            // Assert:
            Assert.IsTrue(searchResponse.Failed);
        }

    }


}