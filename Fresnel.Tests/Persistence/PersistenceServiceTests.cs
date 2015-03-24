using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Fresnel.SampleModel.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Persistence
{
    [TestFixture()]
    public class PersistenceServiceTests
    {
       
        [Test]
        public void ShouldCreateCompleteAggregate()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            var pocoType = typeof(PocoObject);

            // Act:
            var poco = (PocoObject)persistenceService.CreateObject(pocoType);
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();

            var savedChanges = persistenceService.SaveChanges();

            // Assert:
            Assert.IsNotNull(poco);
            Assert.IsTrue(savedChanges > 1);
        }

        [Test]
        public void ShouldCreateSampleData()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            // Act:
            for (var i = 0; i < 5; i++)
            {
                var category = (Category)persistenceService.CreateObject(typeof(Category));
                category.ID = Guid.NewGuid();
                category.Name = "Category " + Environment.TickCount.ToString();

                var money = (Money)persistenceService.CreateObject(typeof(Money));
                money.ID = Guid.NewGuid();
                money.Name = "Money " + Environment.TickCount.ToString();

                var savedChanges = persistenceService.SaveChanges();
            }
        }

        [Test]
        public void ShouldModifyChildCollection()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            var pocoType = typeof(PocoObject);

            // Act:
            var poco = (PocoObject)persistenceService.CreateObject(pocoType);
            poco.ID = Guid.NewGuid();
            poco.AddSomeChildObjects();

            // Step 1:
            var savedChanges1 = persistenceService.SaveChanges();

            // Step 2:
            poco.ChildObjects.Remove(poco.ChildObjects.First());
            var savedChanges2 = persistenceService.SaveChanges();

            // Assert:
            Assert.IsTrue(savedChanges1 > 1);
            Assert.IsTrue(savedChanges2 > 0);

            var persistedPoco = (PocoObject)persistenceService.GetObject(pocoType, poco.ID);
            var differences = poco.ChildObjects.Except(persistedPoco.ChildObjects);
            Assert.AreEqual(0, differences.Count());
        }

        [Test]
        public void ShouldGetObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            var pocoType = typeof(PocoObject);

            for (var i = 0; i < 5; i++)
            {
                var poco = (PocoObject)persistenceService.CreateObject(pocoType);
                poco.ID = Guid.NewGuid();
                poco.AddSomeChildObjects();

                var savedChanges = persistenceService.SaveChanges();
            }

            // Act:
            var pocos = persistenceService.GetObjects(pocoType);

            // Assert:
            Assert.AreNotEqual(0, pocos.OfType<PocoObject>().Count());
        }

        [Test]
        public void ShouldRefreshObjectFromDB()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var dummyText = "This is a test " + DateTime.Now.ToString();

            var persistenceService = container.Resolve<IPersistenceService>();

            var pocoType = typeof(PocoObject);

            var poco = (PocoObject)persistenceService.CreateObject(pocoType);
            poco.ID = Guid.NewGuid();
            poco.NormalText = dummyText;
            poco.AddSomeChildObjects();

            var savedChanges = persistenceService.SaveChanges();

            // Act:
            poco.NormalText = "123456";
            persistenceService.Refresh(poco);

            // Assert:
            Assert.AreEqual(dummyText, poco.NormalText);
        }

    }
}