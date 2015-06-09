using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Northwind.People;
using Envivo.Fresnel.SampleModel.Northwind.Places;
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
using System.Threading;
using TestStack.BDDfy;

namespace Envivo.Fresnel.Tests.Features.Explorer
{
    [TestFixture()]
    public class Cancelling_Save_should_revert_affected_Objects
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private Order _orderPoco;
        private ObjectVM _Order;
        private CancelChangesResponse _CancelChangesResponse;

        public void Given_the_session_is_already_started()
        {
            _TestScopeContainer = new TestScopeContainer(new CustomDependencyModule());

            using (var scope = _TestScopeContainer.BeginScope())
            {
                var engine = _TestScopeContainer.Resolve<Core.Engine>();
                engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

                var sessionController = _TestScopeContainer.Resolve<SessionController>();
                _Session = sessionController.GetSession();
            }
        }

        public void And_given_an_aggregate_root_exists_in_the_database()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                _orderPoco = _Fixture.Create<Order>();
                _orderPoco.PlacedBy = _Fixture.Create<Employee>();
                _orderPoco.PlacedBy.Address = _Fixture.Create<Address>();

                var newEntities = new List<object>() { _orderPoco, _orderPoco.PlacedBy, _orderPoco.PlacedBy.Address };

                var persistenceService = _TestScopeContainer.Resolve<IPersistenceService>();
                persistenceService.SaveChanges(newEntities, new object[0]);

                // This ensures the Object is being observed:
                _TestScopeContainer.Resolve<ObserverCache>().GetObserver(_orderPoco);
                _TestScopeContainer.Resolve<ObserverCache>().GetObserver(_orderPoco.PlacedBy);
                _TestScopeContainer.Resolve<ObserverCache>().GetObserver(_orderPoco.PlacedBy.Address);
            }
        }

        public void When_an_Order_aggregate_root_is_retrieved()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getOrderRequest = new GetObjectRequest()
                {
                    ObjectID = _orderPoco.ID
                };
                var getOrderResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getOrderRequest);

                Assert.IsNotNull(getOrderResponse.ReturnValue);
                _Order = getOrderResponse.ReturnValue;
            }
        }

        public void And_when_the_leaf_Address_is_modified()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var request = new SetPropertyRequest()
                {
                    ObjectID = _orderPoco.PlacedBy.Address.ID,
                    PropertyName = LambdaExtensions.NameOf<Address>(x => x.PostalCode),
                    NonReferenceValue = Guid.NewGuid().ToString()
                };
                var setResponse = _TestScopeContainer.Resolve<ExplorerController>().SetProperty(request);
                Assert.IsTrue(setResponse.Passed);
            }
        }

        public void And_when_the_user_Cancels_the_Address_changes()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var cancelRequest = new CancelChangesRequest()
                {
                    ObjectID = _orderPoco.PlacedBy.Address.ID,
                };

                _CancelChangesResponse = _TestScopeContainer.Resolve<ExplorerController>().CancelChanges(cancelRequest);
                Assert.IsTrue(_CancelChangesResponse.Passed);
            }
        }

        public void Then_the_Order_should_not_have_dirty_contents()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = _orderPoco.ID
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getRequest);

                Assert.IsNotNull(getResponse.ReturnValue);

                var order = getResponse.ReturnValue;
                Assert.IsFalse(order.DirtyState.HasDirtyChildren);
            }
        }

        public void And_then_the_Employee_should_not_have_dirty_contents()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getPropertyRequest = new GetPropertyRequest()
                {
                    ObjectID = _orderPoco.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.PlacedBy),
                };
                var getPropertyResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObjectProperty(getPropertyRequest);
                Assert.IsNotNull(getPropertyResponse.ReturnValue);

                var employee = getPropertyResponse.ReturnValue;
                Assert.IsFalse(employee.DirtyState.IsDirty);
                Assert.IsFalse(employee.DirtyState.HasDirtyChildren);
            }
        }

        public void And_then_the_Address_should_not_have_dirty_contents()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = _orderPoco.PlacedBy.Address.ID
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getRequest);

                var address = getResponse.ReturnValue;
                Assert.IsFalse(address.DirtyState.IsDirty);
                Assert.IsFalse(address.DirtyState.HasDirtyChildren);

                var prop = address.Properties.Single(p => p.InternalName == LambdaExtensions.NameOf<Address>(x => x.PostalCode));
                Assert.AreEqual(_orderPoco.PlacedBy.Address.PostalCode, prop.State.Value);
            }
        }

        public void And_then_the_Response_should_contain_the_affected_object()
        {
            var match = _CancelChangesResponse.CancelledObjects.Single(vm => vm.ID == _orderPoco.PlacedBy.Address.ID);

            var prop = match.Properties.Single(p => p.InternalName == LambdaExtensions.NameOf<Address>(x => x.PostalCode));
            Assert.AreEqual(_orderPoco.PlacedBy.Address.PostalCode, prop.State.Value);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}