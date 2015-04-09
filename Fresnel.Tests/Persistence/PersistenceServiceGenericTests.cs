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
    public class PersistenceServiceGenericTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldCreateCompleteAggregate()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            // Act:
            var category = persistenceService.CreateObject<Category>();
            category.ID = Guid.NewGuid();
            category.Products.AddMany(() => _Fixture.Create<Product>(), 5);

            var savedChanges = persistenceService.SaveChanges();

            // Assert:
            Assert.IsNotNull(category);
            Assert.IsTrue(savedChanges > 1);
        }

        [Test]
        public void ShouldModifyChildCollection()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            // Act:
            var category = persistenceService.CreateObject<Category>();
            category.ID = Guid.NewGuid();
            category.Products.AddMany(() => _Fixture.Create<Product>(), 5);

            // Step 1:
            var savedChanges1 = persistenceService.SaveChanges();

            // Step 2:
            category.Products.Remove(category.Products.First());
            var savedChanges2 = persistenceService.SaveChanges();

            // Assert:
            Assert.IsTrue(savedChanges1 > 1);
            Assert.IsTrue(savedChanges2 > 0);

            var persistedCategory = persistenceService.GetObject<Category>(category.ID);
            var differences = category.Products.Except(persistedCategory.Products);
            Assert.AreEqual(0, differences.Count());
        }

        [Test]
        public void ShouldCreateBiDirectionalLinks()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            // Act:
            var order = persistenceService.CreateObject<Order>();
            order.ID = Guid.NewGuid();

            var orderItem = persistenceService.CreateObject<OrderItem>();
            orderItem.ID = Guid.NewGuid();

            order.OrderItems.Add(orderItem);

            var savedChanges = persistenceService.SaveChanges();

            // Assert:
            Assert.AreEqual(3, savedChanges);

            var persistedA = persistenceService.GetObject<Order>(order.ID);
            var persistedB = persistenceService.GetObject<OrderItem>(orderItem.ID);

            Assert.IsTrue(persistedA.OrderItems.Contains(persistedB));
            Assert.AreEqual(persistedB.ParentOrder, persistedA);
        }

        [Test]
        public void ShouldGetAll()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            var preCategories = persistenceService.GetObjects<Category>().ToList();

            var categoryCount = 5;
            for (var i = 0; i < categoryCount; i++)
            {
                var category = persistenceService.CreateObject<Category>();
                category.ID = Guid.NewGuid();
                category.Products.AddMany(() => _Fixture.Create<Product>(), 3);

                var savedChanges = persistenceService.SaveChanges();
            }

            // Act:
            var postCategories = persistenceService.GetObjects<Category>().ToList();

            // Assert:
            Assert.AreEqual(categoryCount, postCategories.Count() - preCategories.Count());
        }

    }
}