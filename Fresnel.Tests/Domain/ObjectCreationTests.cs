using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using Fresnel.SampleModel.Persistence;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ObjectCreationTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldCreateObjectUsingDefaultCtor()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var createCommand = container.Resolve<CreateObjectCommand>();

            var classType = typeof(Category);

            // Act:
            var oObject = createCommand.Invoke(classType, null);

            // Assert:
            Assert.IsNotNull(oObject);
            var newObject = (Category)oObject.RealObject;
            Assert.IsNotNull(newObject);
        }

        public void ShouldCreateObjectWithCtorArgs()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var createCommand = container.Resolve<CreateObjectCommand>();

            var order = _Fixture.Create<Order>();
            var orderItemType = typeof(OrderItem);

            // Act:
            var oObject = createCommand.Invoke(orderItemType, order);

            // Assert:
            Assert.IsNotNull(oObject);

            var orderItem = (OrderItem)oObject.RealObject;
            Assert.AreEqual(order, orderItem.ParentOrder);
        }

        [Test]
        public void ShouldIgnoreUnusableCtorArgs()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var createCommand = container.Resolve<CreateObjectCommand>();

            // We're attempting to create a new Product, and pass the Category as a potential ctor arg:
            // 'Product' doesn't have a ctor that accepts a Category...
            var category = _Fixture.Create<Category>();
            var productType = typeof(Product);

            // Act:
            var oObject = createCommand.Invoke(productType, category);

            // Assert:
            Assert.IsNotNull(oObject);

            var product = (Product)oObject.RealObject;
            Assert.IsNotNull(product);
        }

        [Test]
        public void ShouldCreateObjectWithDomainFactory()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(ObjectWithCtorInjection).Assembly);

            var createCommand = container.Resolve<CreateObjectCommand>();

            var classType = typeof(Product);

            // Act:
            var oObject = createCommand.Invoke(classType, null);

            // Assert:
            Assert.IsNotNull(oObject);

            var newObject = (Product)oObject.RealObject;
            Assert.AreEqual("This was created using ProductFactory.Create()", newObject.Name);
        }

    }
}