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
    public class PersistenceServiceTests
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

                var categoryType = typeof(Category);

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

                var categoryType = typeof(Category);

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

                var persistedCategory = (Category)persistenceService.GetObject(categoryType, category.ID, new string[0]);
                var differences = category.Products.Except(persistedCategory.Products);
                Assert.AreEqual(0, differences.Count());
            }
        }

        [Test]
        public void ShouldGetObjects()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var persistenceService = _TestScopeContainer.Resolve<IPersistenceService>();

                var categoryType = typeof(Category);

                for (var i = 0; i < 5; i++)
                {
                    var category = _Fixture.Create<Category>();
                    category.Products.AddMany(() => _Fixture.Create<Product>(), 3);

                    var newEntities = new List<object>() { category };
                    newEntities.AddRange(category.Products);

                    var savedChanges = persistenceService.SaveChanges(newEntities, new object[0]);
                }

                // Act:
                var persistedCategories = persistenceService.GetObjects(categoryType, new string[0]).ToList<Category>();

                // Assert:
                Assert.AreNotEqual(0, persistedCategories.Count());
            }
        }

        [Test]
        public void ShouldRefreshObjectFromDB()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var persistenceService = _TestScopeContainer.Resolve<IPersistenceService>();

                var dummyText = _Fixture.Create<string>();
                var product = _Fixture.Create<Product>();
                product.Name = dummyText;

                var newEntities = new List<object>() { product };
                var savedChanges = persistenceService.SaveChanges(newEntities, new object[0]);

                // Act:
                product.Name = _Fixture.Create<string>();
                persistenceService.Refresh(product);

                // Assert:
                Assert.AreEqual(dummyText, product.Name);
            }
        }

    }
}