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
        public void ShouldCreateCompleteAggregate()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var persistenceService = _TestScopeContainer.Resolve<IPersistenceService>();

                // Act:
                var category = _Fixture.Create<Category>();
                category.Products.AddMany(() => _Fixture.Create<Product>(), 5);

                var newEntities = new List<object>() { category };
                newEntities.AddRange(category.Products);

                var savedChanges = persistenceService.SaveChanges(newEntities, new object[0]);

                // Assert:
                Assert.IsNotNull(category);
                Assert.IsTrue(savedChanges > 5);
            }
        }

        [Test]
        public void ShouldModifyChildCollection()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var persistenceService = _TestScopeContainer.Resolve<IPersistenceService>();

                // Act:
                var category = _Fixture.Create<Category>();
                category.Products.AddMany(() => _Fixture.Create<Product>(), 5);

                // Step 1:
                var newEntities = new List<object>() { category };
                newEntities.AddRange(category.Products);

                var savedChanges1 = persistenceService.SaveChanges(newEntities, new object[0]);

                // Step 2:
                var product = category.Products.First();
                category.Products.Remove(product);

                var modifiedEntities = new List<object>() { category, product };

                var savedChanges2 = persistenceService.SaveChanges(new object[0], modifiedEntities);

                // Assert:
                Assert.IsTrue(savedChanges1 > 5);
                Assert.IsTrue(savedChanges2 > 0);

                var persistedCategory = persistenceService.GetObject<Category>(category.ID);
                var differences = category.Products.Except(persistedCategory.Products);
                Assert.AreEqual(0, differences.Count());
            }
        }

        [Test]
        public void ShouldCreateBiDirectionalLinks()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var persistenceService = _TestScopeContainer.Resolve<IPersistenceService>();

                // Act:
                var order = _Fixture.Create<Order>();
                var orderItem1 = _Fixture.Create<OrderItem>();
                orderItem1.ParentOrder = order;
                order.OrderItems.Add(orderItem1);

                var orderItem2 = _Fixture.Create<OrderItem>();
                orderItem2.ParentOrder = order;
                order.OrderItems.Add(orderItem2);

                var newEntities = new List<object>() { order, orderItem1, orderItem2 };

                var savedChanges = persistenceService.SaveChanges(newEntities, new object[0]);

                // Assert:
                Assert.IsTrue(savedChanges > 1);

                var persistedA = persistenceService.GetObject<Order>(order.ID);
                var persistedB = persistenceService.GetObject<OrderItem>(orderItem1.ID);
                var persistedC = persistenceService.GetObject<OrderItem>(orderItem2.ID);

                Assert.IsTrue(persistedA.OrderItems.Contains(persistedB));
                Assert.IsTrue(persistedA.OrderItems.Contains(persistedC));
                Assert.AreEqual(persistedB.ParentOrder, persistedA);
                Assert.AreEqual(persistedC.ParentOrder, persistedA);
            }
        }

        [Test]
        public void ShouldGetAll()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var persistenceService = _TestScopeContainer.Resolve<IPersistenceService>();

                var preCategories = persistenceService.GetObjects<Category>().ToList();

                var categoryCount = 5;
                for (var i = 0; i < categoryCount; i++)
                {
                    var category = _Fixture.Create<Category>();
                    category.Products.AddMany(() => _Fixture.Create<Product>(), 3);

                    var newEntities = new List<object>() { category };
                    newEntities.AddRange(category.Products);

                    var savedChanges = persistenceService.SaveChanges(newEntities, new object[0]);
                }

                // Act:
                var postCategories = persistenceService.GetObjects<Category>().ToList();

                // Assert:
                Assert.AreEqual(categoryCount, postCategories.Count() - preCategories.Count());
            }
        }

    }
}