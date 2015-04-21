using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.SampleModel.Factories;
using Fresnel.SampleModel.Persistence;
using NUnit.Framework;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class MethodTests
    {

        [Test]
        public void ShouldInvokeMethodWithMultipleParameters()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MethodSamples).Assembly);

            var observerCache = container.Resolve<ObserverCache>();
            var controller = container.Resolve<ExplorerController>();

            var obj = container.Resolve<MethodSamples>();
            obj.ID = Guid.NewGuid();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var request = new InvokeMethodRequest()
            {
                ObjectID = obj.ID,
                MethodName = "MethodWithValueParameters",
                Parameters = new ParameterVM[] { 
                    new ParameterVM(){ InternalName = "aString", State = new ValueStateVM() { Value = "123"} },
                    new ParameterVM(){ InternalName = "aNumber", State = new ValueStateVM() { Value = 123} },
                    new ParameterVM(){ InternalName = "aDate", State = new ValueStateVM() { Value = DateTime.Now} },
                 }
            };

            var response = controller.InvokeMethod(request);

            // Assert:
            Assert.IsTrue(response.Passed);
        }

        [Test]
        public void ShouldInvokeMethodWithObjectParameters()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MethodSamples).Assembly);

            var observerCache = container.Resolve<ObserverCache>();
            var toolboxController = container.Resolve<ToolboxController>();
            var explorerController = container.Resolve<ExplorerController>();

            var obj = container.Resolve<MethodSamples>();
            obj.ID = Guid.NewGuid();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var createRequest = new CreateObjectRequest()
            {
                ClassTypeName = typeof(MethodSamples).FullName
            };
            var createResponse = toolboxController.Create(createRequest);

            var methodName = "MethodWithObjectParameters";
            var parameterName = "category";

            // Find some objects that can be used for the Parameter:
            var searchRequest = new SearchParameterRequest()
            {
                ObjectID = obj.ID,
                MethodName = methodName,
                ParameterName = parameterName,
                OrderBy = "",
                IsDescendingOrder = true,
                PageSize = 100,
                PageNumber = 1
            };
            var searchResponse = explorerController.SearchParameterObjects(searchRequest);
            var parameterValue = searchResponse.Result.Items.First();

            // Now set the Parameter:
            var setRequest = new SetParameterRequest()
            {
                ObjectID = obj.ID,
                MethodName = methodName,
                ParameterName = parameterName,
                ReferenceValueId = parameterValue.ID
            };
            var setResponse = explorerController.SetParameter(setRequest);

            // And finally invoke the Method:
            var invokeRequest = new InvokeMethodRequest()
            {
                ObjectID = obj.ID,
                MethodName = "MethodWithObjectParameters",
            };

            var invokeResponse = explorerController.InvokeMethod(invokeRequest);

            // Assert:
            Assert.IsTrue(invokeResponse.Passed);
        }

        [Test]
        public void ShouldInvokeFactoryMethod()
        {
            // Arrange:
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(MethodSamples).Assembly);

            var observerCache = container.Resolve<ObserverCache>();
            var toolboxController = container.Resolve<ToolboxController>();

            var obj = container.Resolve<MethodSamples>();
            obj.ID = Guid.NewGuid();
            var oObject = (ObjectObserver)observerCache.GetObserver(obj);

            // Act:
            var invokeRequest = new InvokeMethodRequest()
            {
                ClassType = typeof(ProductFactory).FullName,
                MethodName = "Create",
            };

            var invokeResponse = toolboxController.InvokeDependencyMethod(invokeRequest);

            // Assert:
            Assert.IsTrue(invokeResponse.Passed);
            var objectVM = invokeResponse.ResultObject;

            var nameProp = objectVM.Properties.Single(p=> p.InternalName == "Name");
            Assert.AreEqual("This was created using ProductFactory.Create()", nameProp.State.Value);
        }

    }
}