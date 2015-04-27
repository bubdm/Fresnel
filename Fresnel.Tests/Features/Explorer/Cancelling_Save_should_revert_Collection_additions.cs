using Autofac;
using Envivo.Fresnel.CompositionRoot;
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
using System;
using System.Linq;
using System.Threading;
using TestStack.BDDfy;

namespace Envivo.Fresnel.Tests.Features.Explorer
{
    [TestFixture()]
    public class Cancelling_Save_should_revert_Collection_additions
    {
        private TestScopeContainer _TestScopeContainer = null;

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

        public void And_given_an_Order_is_loaded()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var searchRequest = new SearchObjectsRequest()
                {
                    SearchType = typeof(Order).FullName,
                    PageSize = 10,
                    PageNumber = 1,
                };
                var searchResponse = _TestScopeContainer.Resolve<ToolboxController>().SearchObjects(searchRequest);
                Assert.IsTrue(searchResponse.Passed);
                _Order = searchResponse.Result.Items.First();
            }
        }

        public void When_an_OrderItem_is_added_to_the_parent()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var request = new CollectionAddNewRequest()
                {
                    ParentObjectID = _Order.ID,
                    CollectionPropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                    ElementTypeName = typeof(OrderItem).FullName,
                };
                var createAddResponse = _TestScopeContainer.Resolve<ExplorerController>().AddNewItemToCollection(request);
                Assert.IsTrue(createAddResponse.Passed);

                _OrderItem = createAddResponse.Modifications.NewObjects.Last();
            }
        }

        public void And_when_the_OrderItem_is_modified()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var request = new SetPropertyRequest()
                {
                    ObjectID = _OrderItem.ID,
                    PropertyName = LambdaExtensions.NameOf<OrderItem>(x => x.Quantity),
                    NonReferenceValue = 1234
                };
                var setResponse = _TestScopeContainer.Resolve<ExplorerController>().SetProperty(request);
                Assert.IsTrue(setResponse.Passed);
            }
        }

        public void And_when_the_user_Cancels_the_OrderItem()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var cancelRequest = new CancelChangesRequest()
                {
                    ObjectID = _OrderItem.ID,
                };

                var cancelResponse = _TestScopeContainer.Resolve<ExplorerController>().CancelChanges(cancelRequest);
                Assert.IsTrue(cancelResponse.Passed);
            }
        }

        public void And_when_the_user_Cancels_the_Save_on_the_OrderItems_Collection()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var cancelRequest = new CancelChangesRequest()
                {
                    ObjectID = _Order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                };

                var cancelResponse = _TestScopeContainer.Resolve<ExplorerController>().CancelChanges(cancelRequest);
                Assert.IsTrue(cancelResponse.Passed);
            }
        }

        public void Then_the_Order_should_not_have_dirty_contents()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = _Order.ID
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getRequest);

                Assert.IsNotNull(getResponse.ReturnValue);
            }

            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getPropertyRequest = new GetPropertyRequest()
                {
                    ObjectID = _Order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                };
                var getPropertyResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObjectProperty(getPropertyRequest);

                Assert.IsNotNull(getPropertyResponse.ReturnValue);

                var collectionVM = (CollectionVM)getPropertyResponse.ReturnValue;
                Assert.IsFalse(collectionVM.DirtyState.IsDirty);
                Assert.IsFalse(collectionVM.DirtyState.HasDirtyChildren);

                var matchingChild = collectionVM.Items.SingleOrDefault(i => i.ID == _OrderItem.ID);
                Assert.IsNull(matchingChild);
            }
        }

        public void And_then_the_OrderItem_should_not_have_dirty_contents()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = _OrderItem.ID
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getRequest);

                var prop = getResponse.ReturnValue.Properties.Single(p => p.InternalName == LambdaExtensions.NameOf<OrderItem>(x => x.Quantity));
                Assert.AreNotEqual(1234, prop.State.Value);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}