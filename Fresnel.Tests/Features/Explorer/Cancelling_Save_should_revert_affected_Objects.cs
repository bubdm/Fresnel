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
        private IContainer _Container;
        private ToolboxController _ToolboxController;
        private ExplorerController _ExplorerController;

        private SessionVM _Session;
        private Order _orderPoco;
        private ObjectVM _Order;

        public void Given_the_session_is_already_started()
        {
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            _Container = new ContainerFactory().Build(customDependencyModules);

            var engine = _Container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            _ToolboxController = _Container.Resolve<ToolboxController>();
            _ExplorerController = _Container.Resolve<ExplorerController>();

            var sessionController = _Container.Resolve<SessionController>();
            _Session = sessionController.GetSession();
        }

        public void And_given_an_aggregate_root_exists_in_the_database()
        {
            _orderPoco = _Fixture.Create<Order>();
            _orderPoco.PlacedBy = _Fixture.Create<Employee>();
            _orderPoco.PlacedBy.Address = _Fixture.Create<Address>();

            var newEntities = new List<object>() { _orderPoco, _orderPoco.PlacedBy, _orderPoco.PlacedBy.Address };

            var persistenceService = _Container.Resolve<IPersistenceService>();
            persistenceService.SaveChanges(newEntities, new object[0]);

            // This ensures the Object is being observed:
            _Container.Resolve<ObserverCache>().GetObserver(_orderPoco);
            _Container.Resolve<ObserverCache>().GetObserver(_orderPoco.PlacedBy);
            _Container.Resolve<ObserverCache>().GetObserver(_orderPoco.PlacedBy.Address);
        }

        public void When_an_Order_aggregate_root_is_retrieved()
        {
            var getOrderRequest = new GetObjectRequest()
            {
                ObjectID = _orderPoco.ID
            };
            var getOrderResponse = _ExplorerController.GetObject(getOrderRequest);

            Assert.IsNotNull(getOrderResponse.ReturnValue);
            _Order = getOrderResponse.ReturnValue;
        }

        public void And_when_the_leaf_Address_is_modified()
        {
            var request = new SetPropertyRequest()
            {
                ObjectID = _orderPoco.PlacedBy.Address.ID,
                PropertyName = LambdaExtensions.NameOf<Address>(x => x.PostalCode),
                NonReferenceValue = Guid.NewGuid().ToString()
            };
            var setResponse = _ExplorerController.SetProperty(request);
            Assert.IsTrue(setResponse.Passed);
        }

        public void And_when_the_user_Cancels_the_Address_changes()
        {
            var cancelRequest = new CancelChangesRequest()
            {
                ObjectID = _orderPoco.PlacedBy.Address.ID,
            };

            var cancelResponse = _ExplorerController.CancelChanges(cancelRequest);
            Assert.IsTrue(cancelResponse.Passed);
        }

        public void Then_the_Order_should_not_have_dirty_contents()
        {
            var getRequest = new GetObjectRequest()
            {
                ObjectID = _orderPoco.ID
            };
            var getResponse = _ExplorerController.GetObject(getRequest);

            Assert.IsNotNull(getResponse.ReturnValue);

            var order = getResponse.ReturnValue;
            Assert.IsFalse(order.DirtyState.IsDirty);
            Assert.IsFalse(order.DirtyState.HasDirtyChildren);
        }

        public void And_then_the_Employee_should_not_have_dirty_contents()
        {
            var getPropertyRequest = new GetPropertyRequest()
            {
                ObjectID = _orderPoco.ID,
                PropertyName = LambdaExtensions.NameOf<Order>(x => x.PlacedBy),
            };
            var getPropertyResponse = _ExplorerController.GetObjectProperty(getPropertyRequest);
            Assert.IsNotNull(getPropertyResponse.ReturnValue);

            var employee = getPropertyResponse.ReturnValue;
            Assert.IsFalse(employee.DirtyState.IsDirty);
            Assert.IsFalse(employee.DirtyState.HasDirtyChildren);
        }

        public void And_then_the_Address_should_not_have_dirty_contents()
        {
            var getRequest = new GetObjectRequest()
            {
                ObjectID = _orderPoco.PlacedBy.Address.ID
            };
            var getResponse = _ExplorerController.GetObject(getRequest);

            var address = getResponse.ReturnValue;
            Assert.IsFalse(address.DirtyState.IsDirty);
            Assert.IsFalse(address.DirtyState.HasDirtyChildren);

            var prop = address.Properties.Single(p => p.InternalName == LambdaExtensions.NameOf<Address>(x => x.PostalCode));
            Assert.AreEqual(_orderPoco.PlacedBy.Address.PostalCode, prop.State.Value);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}