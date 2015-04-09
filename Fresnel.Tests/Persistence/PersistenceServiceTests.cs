﻿using Autofac;
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

        [Test]
        public void ShouldCreateCompleteAggregate()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            var categoryType = typeof(Category);

            // Act:
            var category = _Fixture.Create<Category>();
            category.Products.AddMany(() => _Fixture.Create<Product>(), 5);

            var savedChanges = persistenceService.SaveChanges(category);

            // Assert:
            Assert.IsNotNull(category);
            Assert.IsTrue(savedChanges > 5);
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

            var categoryType = typeof(Category);

            // Act:
            var category = _Fixture.Create<Category>();
            category.Products.AddMany(() => _Fixture.Create<Product>(), 5);

            // Step 1:
            var savedChanges1 = persistenceService.SaveChanges(category);

            // Step 2:
            category.Products.Remove(category.Products.First());
            var savedChanges2 = persistenceService.SaveChanges(category);

            // Assert:
            Assert.IsTrue(savedChanges1 > 5);
            Assert.IsTrue(savedChanges2 > 0);

            var persistedCategory = (Category)persistenceService.GetObject(categoryType, category.ID);
            var differences = category.Products.Except(persistedCategory.Products);
            Assert.AreEqual(0, differences.Count());
        }

        [Test]
        public void ShouldGetObjects()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            var categoryType = typeof(Category);

            for (var i = 0; i < 5; i++)
            {
                var category = _Fixture.Create<Category>();
                category.Products.AddMany(() => _Fixture.Create<Product>(), 3);
                var savedChanges = persistenceService.SaveChanges(category);
            }

            // Act:
            var persistedCategories = persistenceService.GetObjects(categoryType).ToList<Category>();

            // Assert:
            Assert.AreNotEqual(0, persistedCategories.Count());
        }

        [Test]
        public void ShouldRefreshObjectFromDB()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var persistenceService = container.Resolve<IPersistenceService>();

            var dummyText = _Fixture.Create<string>();
            var product = _Fixture.Create<Product>();
            product.Name = dummyText;

            var savedChanges = persistenceService.SaveChanges(product);

            // Act:
            product.Name = _Fixture.Create<string>();
            persistenceService.Refresh(product);

            // Assert:
            Assert.AreEqual(dummyText, product.Name);
        }

    }
}