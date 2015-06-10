using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.SampleModel.Factories;
using Fresnel.SampleModel.Persistence;
using Fresnel.Tests;
using NUnit.Framework;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Features
{
    [TestFixture()]
    public class MethodTests
    {
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
        public void ShouldInvokeMethodWithMultipleParameters()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var controller = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var obj = _TestScopeContainer.Resolve<MethodSamples>();
                obj.ID = Guid.NewGuid();
                var oObject = (ObjectObserver)observerRetriever.GetObserver(obj);

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
        }

        [Test]
        public void ShouldInvokeMethodWithObjectParameters()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var toolboxController = _TestScopeContainer.Resolve<ToolboxController>();
                var explorerController = _TestScopeContainer.Resolve<ExplorerController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var obj = _TestScopeContainer.Resolve<MethodSamples>();
                obj.ID = Guid.NewGuid();
                var oObject = (ObjectObserver)observerRetriever.GetObserver(obj);

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
        }

        [Test]
        public void ShouldInvokeFactoryMethod()
        {
            // Arrange:
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var observerRetriever = _TestScopeContainer.Resolve<ObserverRetriever>();
                var toolboxController = _TestScopeContainer.Resolve<ToolboxController>();

                _TestScopeContainer.Resolve<ObserverCache>().CleanUp();
                var factory = _TestScopeContainer.Resolve<IFactory<Product>>();
                var oFactory = observerRetriever.GetObserver(factory);


                // Act:
                var invokeRequest = new InvokeMethodRequest()
                {
                    ObjectID = oFactory.ID,
                    MethodName = "Create"
                };

                var invokeResponse = toolboxController.InvokeMethod(invokeRequest);

                // Assert:
                Assert.IsTrue(invokeResponse.Passed);
                var objectVM = invokeResponse.ResultObject;

                var nameProp = objectVM.Properties.Single(p => p.InternalName == "Name");
                Assert.AreEqual("This was created using ProductFactory.Create()", nameProp.State.Value);
            }
        }

    }
}