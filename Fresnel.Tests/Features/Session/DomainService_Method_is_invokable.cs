using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Classes;
using Envivo.Fresnel.Utils;
using Fresnel.SampleModel.Persistence;
using Fresnel.Tests;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using TestStack.BDDfy;
using Envivo.Fresnel.SampleModel.Northwind.Services;
using Envivo.Fresnel.SampleModel.Northwind.People;

namespace Envivo.Fresnel.Tests.Features.Session
{
    [TestFixture()]
    public class DomainService_Method_should_invoke
    {
        private TestScopeContainer _TestScopeContainer = null;

        private readonly Type _ServiceClass = typeof(OrderPlacementService);
        private readonly string _ServiceMethodName = "PlaceNewOrder";

        private SessionVM _Session;
        private ClassItem _DomainServiceClass;
        private MethodVM _ServiceMethod;

        private Guid _Parameter1_CustomerId;
        private Guid _Parameter2_ProductId;
        private int _Parameter3_Quantity;

        public void Given_the_session_is_already_started()
        {
            _TestScopeContainer = new TestScopeContainer(new CustomDependencyModule());

            using (var scope = _TestScopeContainer.BeginScope())
            {
                var engine = _TestScopeContainer.Resolve<Core.Engine>();
                engine.RegisterDomainAssembly(typeof(OrderPlacementService).Assembly);

                var sessionController = _TestScopeContainer.Resolve<SessionController>();
                _Session = sessionController.GetSession();
            }
        }

        public void And_given_that_a_DomainServices_is_available()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var toolboxController = _TestScopeContainer.Resolve<ToolboxController>();

                var getDomainLibraryResponse = toolboxController.GetDomainLibrary();

                var domainServicesHierarchy = getDomainLibraryResponse.DomainServices;
                Assert.AreNotEqual(0, domainServicesHierarchy.Count());

                var serviceClasses = domainServicesHierarchy.SelectMany(ns => ns.Classes);
                _DomainServiceClass = serviceClasses.Single(s => s.FullTypeName == typeof(OrderPlacementService).FullName);

                Assert.IsNotNull(_DomainServiceClass);
            }
        }

        public void And_given_that_a_Method_is_selected_from_the_DomainService()
        {
            _ServiceMethod = _DomainServiceClass.ServiceMethods.Single(m => m.InternalName == _ServiceMethodName);
        }

        public void When_the_first_Method_Parameter_is_set()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var explorerController = _TestScopeContainer.Resolve<ExplorerController>();

                _Parameter1_CustomerId = this.GetIdForExisting<Customer>();

                var setParameterRequest = new SetParameterRequest
                {
                    ClassFullTypeName = _ServiceClass.FullName,
                    MethodName = _ServiceMethodName,
                    ParameterName = "customer",
                    ReferenceValueId = _Parameter1_CustomerId,
                };

                explorerController.SetParameter(setParameterRequest);
            }
        }

        public void And_when_the_second_Method_Parameter_is_set()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var explorerController = _TestScopeContainer.Resolve<ExplorerController>();

                _Parameter2_ProductId = this.GetIdForExisting<Product>();

                var setParameterRequest = new SetParameterRequest
                {
                    ClassFullTypeName = _ServiceClass.FullName,
                    MethodName = _ServiceMethodName,
                    ParameterName = "product",
                    ReferenceValueId = _Parameter2_ProductId,
                };

                explorerController.SetParameter(setParameterRequest);
            }
        }

        public void And_when_the_third_Method_Parameter_is_set()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var explorerController = _TestScopeContainer.Resolve<ExplorerController>();

                _Parameter3_Quantity = 3;

                var setParameterRequest = new SetParameterRequest
                {
                    ClassFullTypeName = _ServiceClass.FullName,
                    MethodName = _ServiceMethodName,
                    ParameterName = "quantity",
                    NonReferenceValue = _Parameter3_Quantity
                };

                explorerController.SetParameter(setParameterRequest);
            }
        }

        public void Then_the_Method_should_be_invoked()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var explorerController = _TestScopeContainer.Resolve<ExplorerController>();

                var parameters = new ParameterVM[]
                {
                    new ParameterVM
                    { 
                        InternalName = "customer",
                        State = new ValueStateVM 
                        { 
                            //ValueType = typeof(Customer).Name,
                            ReferenceValueID = _Parameter1_CustomerId
                        } 
                    },
                    new ParameterVM
                    { 
                        InternalName = "product",
                        State = new ValueStateVM 
                        {
                            //ValueType = typeof(Product).Name,
                            ReferenceValueID = _Parameter2_ProductId
                        } 
                    },
                    new ParameterVM
                    { 
                        InternalName = "quantity",
                        State = new ValueStateVM 
                        {
                            //ValueType = typeof(int).Name,
                            Value = _Parameter3_Quantity
                        } 
                    },
                };

                var invokeMethodRequest = new InvokeMethodRequest
                {
                    ClassFullTypeName = _ServiceClass.FullName,
                    MethodName = _ServiceMethodName,
                    Parameters = parameters
                };

                var response = explorerController.InvokeMethod(invokeMethodRequest);

                Assert.IsTrue(response.Passed);
                Assert.IsNotNull(response.ResultObject);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

        private Guid GetIdForExisting<TPersistable>()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var toolboxController = _TestScopeContainer.Resolve<ToolboxController>();

                var searchRequest = new SearchObjectsRequest()
                {
                    SearchType = typeof(TPersistable).FullName,
                    IsDescendingOrder = true,
                    PageSize = 100,
                    PageNumber = 1
                };

                var searchResponse = toolboxController.SearchObjects(searchRequest);

                var customer = searchResponse.Result.Items.Last();
                return customer.ID;
            }
        }

    }
}