using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Persistence
{
    [TestFixture()]
    public class SearchTests
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
        public void ShouldFetchObjectsForClass()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var templateCache = _TestScopeContainer.Resolve<TemplateCache>();
                var searchCommand = _TestScopeContainer.Resolve<SearchCommand>();

                var tClass = (ClassTemplate)templateCache.GetTemplate<Order>();

                // Act:
                var results = searchCommand.Search(tClass).ToList<Order>();

                // Assert:
                Assert.IsNotNull(results);
                Assert.AreNotEqual(0, results.Count());
            }
        }

        [Test]
        public void ShouldFetchObjectsForPropertyWithQuerySpecification()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var searchCommand = _TestScopeContainer.Resolve<SearchCommand>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var obj = _TestScopeContainer.Resolve<ObjectWithCtorInjection>();
                var oObj = (ObjectObserver)observerRetriever.GetObserver(obj);

                // This property has the QuerySpecification associated with it:
                var propName = LambdaExtensions.NameOf<ObjectWithCtorInjection>(x => x.Product);
                var oProp = oObj.Properties[propName];

                // Act:
                var results = searchCommand.Search(oProp).ToList<Product>();

                // Assert:
                Assert.IsNotNull(results);
                foreach (var item in results)
                {
                    Assert.IsFalse(item.Name.Contains("Test"));
                }
            }
        }

        [Test]
        public void ShouldFetchObjectsForPropertyWithoutQuerySpecification()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                // Arrange:
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var searchCommand = _TestScopeContainer.Resolve<SearchCommand>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var obj = _TestScopeContainer.Resolve<Product>();
                var oObj = (ObjectObserver)observerRetriever.GetObserver(obj);
                var propName = LambdaExtensions.NameOf<Product>(x => x.Categories);
                var oProp = oObj.Properties[propName];

                // Act:
                var results = searchCommand.Search(oProp).ToList<Category>();

                // Assert:
                Assert.IsNotNull(results);
                Assert.AreNotEqual(0, results.Count());
            }
        }

    }
}