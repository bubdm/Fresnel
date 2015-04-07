using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Fresnel.SampleModel.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class ObjectCreationTests
    {
        [Test]
        public void ShouldCreateObjectUsingDefaultCtor()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var createCommand = container.Resolve<CreateObjectCommand>();

            var classType = typeof(SampleModel.Objects.Money);

            // Act:
            var oObject = createCommand.Invoke(classType, null);

            // Assert:
            Assert.IsNotNull(oObject);
            var newObject = (SampleModel.Objects.Money)oObject.RealObject;
            Assert.IsNotNull(newObject);
        }

        public void ShouldCreateObjectWithCtorArgs()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var createCommand = container.Resolve<CreateObjectCommand>();

            var masterObject = new SampleModel.Objects.MasterObject();
            var detailType = typeof(SampleModel.Objects.DetailObject);

            // Act:
            var oObject = createCommand.Invoke(detailType, new object[] { masterObject });

            // Assert:
            Assert.IsNotNull(oObject);

            var detailObj = (SampleModel.Objects.DetailObject)oObject.RealObject;
            Assert.AreEqual(masterObject, detailObj.Parent);
        }

        public void ShouldCreateObjectWithDomainFactory()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var createCommand = container.Resolve<CreateObjectCommand>();

            var classType = typeof(SampleModel.Objects.PocoObject);

            // Act:
            var oObject = createCommand.Invoke(classType, null);

            // Assert:
            Assert.IsNotNull(oObject);

            var newObject = (SampleModel.Objects.PocoObject)oObject.RealObject;
            Assert.AreEqual("This was created using PocoObjectFactory.Create()", newObject.NormalText);
        }

        public void ShouldCreateObjectWithIoCFactory()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var createCommand = container.Resolve<CreateObjectCommand>();

            var classType = typeof(SampleModel.TestTypes.ObjectWithCtorInjection);
            var testName = "Test " + DateTime.Now.Ticks.ToString();

            // Act:
            var oObject = createCommand.Invoke(classType, new object[] { testName });

            // Assert:
            Assert.IsNotNull(oObject);

            var newObject = (SampleModel.TestTypes.ObjectWithCtorInjection)oObject.RealObject;
            Assert.AreEqual(testName, newObject.Name);
        }

        public void ShouldCreateObjectWithPersistenceService()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var createCommand = container.Resolve<CreateObjectCommand>();

            var classType = typeof(SampleModel.Objects.Category);

            // Act:
            var oObject = createCommand.Invoke(classType, null);

            // Assert:
            Assert.IsNotNull(oObject);

            var newObject = (SampleModel.Objects.Category)oObject.RealObject;
            Assert.IsTrue(newObject.GetType().Assembly.IsDynamic); // ie it's an EF Proxy
        }
    }
}