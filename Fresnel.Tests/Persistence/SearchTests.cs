using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;

using Fresnel.SampleModel.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Persistence
{
    [TestFixture()]
    public class SearchTests
    {

        [Test]
        public void ShouldFetchObjectsForClass()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var templateCache = container.Resolve<TemplateCache>();
            var searchCommand = container.Resolve<SearchCommand>();

            var tClass = (ClassTemplate)templateCache.GetTemplate<SampleModel.Objects.PocoObject>();

            // Act:
            var results = searchCommand.Search(tClass).ToList<SampleModel.Objects.PocoObject>();

            // Assert:
            Assert.IsNotNull(results);
            Assert.AreNotEqual(0, results.Count());
        }

        [Test]
        public void ShouldFetchObjectsForPropertyWithQuerySpecification()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var observerCache = container.Resolve<ObserverCache>();
            var searchCommand = container.Resolve<SearchCommand>();

            var obj = container.Resolve<SampleModel.Objects.DependencyAwareObject>();
            var oObj = (ObjectObserver)observerCache.GetObserver(obj);
            var oProp = oObj.Properties["PocoObject"];

            // Act:
            var results = searchCommand.Search(oProp).ToList<SampleModel.Objects.PocoObject>();

            // Assert:
            Assert.IsNotNull(results);
            foreach (var item in results)
            {
                Assert.IsFalse(item.NormalText.Contains("Test"));
            }
        }

        [Test]
        public void ShouldFetchObjectsForPropertyWithoutQuerySpecification()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var observerCache = container.Resolve<ObserverCache>();
            var searchCommand = container.Resolve<SearchCommand>();

            var obj = container.Resolve<SampleModel.Objects.PocoObject>();
            var oObj = (ObjectObserver)observerCache.GetObserver(obj);
            var oProp = oObj.Properties["ChildObjects"];

            // Act:
            var results = searchCommand.Search(oProp).ToList<SampleModel.Objects.PocoObject>();

            // Assert:
            Assert.IsNotNull(results);
            Assert.AreNotEqual(0, results.Count());
        }

    }
}