using Autofac;
using Autofac.Integration.WebApi;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using Fresnel.SampleModel.Persistence;
using Fresnel.Tests;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Dependencies;
using TestStack.BDDfy;

namespace Envivo.Fresnel.Tests.Features.Explorer
{
    [TestFixture()]
    public class Saving_transient_Aggregates
    {
        private TestScopeContainer _TestScopeContainer = null;

        // For debugging only:
        private ObjectObserver _oOrder;

        private SessionVM _Session;
        private ObjectVM _Order;
        private ObjectVM _OrderItem;

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

        public void And_given_an_Order_is_created()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var createRequest = new CreateObjectRequest()
                {
                    ClassTypeName = typeof(Order).FullName
                };
                var createResponse = _TestScopeContainer.Resolve<ToolboxController>().Create(createRequest);
                _Order = createResponse.NewObject;

                // For debugging only:
                var observerCache = _TestScopeContainer.Resolve<Core.Observers.ObserverCache>();
                _oOrder = (Core.Observers.ObjectObserver)observerCache.GetObserverById(_Order.ID);
            }
        }

        public void And_given_the_OrderItems_property_is_loaded()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetPropertyRequest()
                {
                    ObjectID = _Order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems)
                };
                var getResult = _TestScopeContainer.Resolve<ExplorerController>().GetObjectProperty(getRequest);
            }
        }

        public void When_a_new_OrderItem_is_added_to_the_OrderItems_Collection()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var orderItemType = typeof(OrderItem);
                var collectionAddNewRequest = new CollectionAddNewRequest()
                {
                    ParentObjectID = _Order.ID,
                    CollectionPropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                    ElementTypeName = orderItemType.FullName
                };
                var collectionAddResponse = _TestScopeContainer.Resolve<ExplorerController>().AddNewItemToCollection(collectionAddNewRequest);

                _OrderItem = collectionAddResponse.Modifications.NewObjects.Single(o => o.Type == typeof(OrderItem).Name);
            }
        }

        public void And_when_the_user_Saves_the_Order()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var saveRequest = new SaveChangesRequest()
                {
                    ObjectID = _Order.ID
                };
                var saveResponse = _TestScopeContainer.Resolve<ExplorerController>().SaveChanges(saveRequest);
                Assert.AreEqual(2, saveResponse.SavedObjects.Count());
            }
        }

        public void Then_the_Order_should_be_saved_completely()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = _Order.ID
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getRequest);

                var getPropRequest = new GetPropertyRequest()
                {
                    ObjectID = _Order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems)
                };
                var getPropResult = _TestScopeContainer.Resolve<ExplorerController>().GetObjectProperty(getPropRequest);

                var orderItems = (CollectionVM)getPropResult.ReturnValue;
                var match = orderItems.Items.SingleOrDefault(o => o.ID == _OrderItem.ID);
                Assert.IsNotNull(match);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}