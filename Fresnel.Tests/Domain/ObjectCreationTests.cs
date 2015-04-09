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
            var oObject = createCommand.Invoke(orderItemType, new object[] { order });

            // Assert:
            Assert.IsNotNull(oObject);

            var orderItem = (OrderItem)oObject.RealObject;
            Assert.AreEqual(order, orderItem.ParentOrder);
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

        [Test]
        public void ShouldCreateObjectWithPersistenceService()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(ObjectWithCtorInjection).Assembly);

            var createCommand = container.Resolve<CreateObjectCommand>();

            var classType = typeof(Category);

            // Act:
            var oObject = createCommand.Invoke(classType, null);

            // Assert:
            Assert.IsNotNull(oObject);

            var newObject = (Category)oObject.RealObject;
            Assert.IsTrue(newObject.GetType().Assembly.IsDynamic); // ie it's an EF Proxy
        }
    }
}