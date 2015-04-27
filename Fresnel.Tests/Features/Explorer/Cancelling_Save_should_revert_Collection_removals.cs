using Autofac;
using Autofac.Integration.WebApi;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using Fresnel.SampleModel.Persistence;
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
    public class Cancelling_Save_should_revert_Collection_removals
    {
        private HttpConfiguration _HttpConfiguration = null;

        private SessionVM _Session;
        private ObjectVM _Order;
        private CollectionVM _OrderItems;
        private ObjectVM _OrderItem;

        private T GetService<T>(IDependencyScope scope)
        {
            return (T)scope.GetService(typeof(T));
        }

        public void Given_the_session_is_already_started()
        {
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            var container = new ContainerFactory().Build(customDependencyModules);
            _HttpConfiguration = new HttpConfiguration
            {
                DependencyResolver = new AutofacWebApiDependencyResolver(container)
            };

            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var engine = this.GetService<Core.Engine>(scope);
                engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

                var sessionController = this.GetService<SessionController>(scope);
                _Session = sessionController.GetSession();
            }
        }

        public void And_given_an_Order_is_loaded()
        {
            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var searchRequest = new SearchObjectsRequest()
                {
                    SearchType = typeof(Order).FullName,
                    PageSize = 10,
                    PageNumber = 1,
                };
                var searchResponse = this.GetService<ToolboxController>(scope).SearchObjects(searchRequest);
                Assert.IsTrue(searchResponse.Passed);
                _Order = searchResponse.Result.Items.First();
            }

        }

        public void And_given_the_OrderItems_property_is_loaded()
        {
            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var getPropertyRequest = new GetPropertyRequest()
                {
                    ObjectID = _Order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                };
                var getPropertyResponse = this.GetService<ExplorerController>(scope).GetObjectProperty(getPropertyRequest);

                _OrderItems = (CollectionVM)getPropertyResponse.ReturnValue;
                _OrderItem = _OrderItems.Items.Last();
            }
        }

        public void When_an_OrderItem_is_removed_from_the_OrderItems_Collection()
        {
            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var request = new CollectionRemoveRequest()
                {
                    ParentObjectID = _Order.ID,
                    CollectionPropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                    ElementID = _OrderItem.ID,
                };
                var removeResponse = this.GetService<ExplorerController>(scope).RemoveItemFromCollection(request);
                Assert.IsTrue(removeResponse.Passed);
            }
        }

        public void And_when_the_user_Cancels_the_Save_on_the_OrderItems_Collection()
        {
            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var cancelRequest = new CancelChangesRequest()
                {
                    ObjectID = _Order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                };

                var cancelResponse = this.GetService<ExplorerController>(scope).CancelChanges(cancelRequest);
                Assert.IsTrue(cancelResponse.Passed);
            }
        }

        public void And_when_the_user_Cancels_the_Save_on_the_Order()
        {
            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var cancelRequest = new CancelChangesRequest()
                {
                    ObjectID = _Order.ID,
                };

                var cancelResponse = this.GetService<ExplorerController>(scope).CancelChanges(cancelRequest);
                Assert.IsTrue(cancelResponse.Passed);
            }
        }

        public void Then_the_Order_should_not_have_dirty_contents()
        {
            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = _Order.ID
                };
                var getResponse = this.GetService<ExplorerController>(scope).GetObject(getRequest);
                Assert.IsNotNull(getResponse.ReturnValue);

                var objectVM = getResponse.ReturnValue;
                Assert.IsFalse(objectVM.DirtyState.IsDirty);
                Assert.IsFalse(objectVM.DirtyState.HasDirtyChildren);
            }
        }

        public void Then_the_Orders_OrderItems_should_have_the_original_contents()
        {
            using (var scope = _HttpConfiguration.DependencyResolver.BeginScope())
            {
                var getPropertyRequest = new GetPropertyRequest()
                {
                    ObjectID = _Order.ID,
                    PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                };
                var getPropertyResponse = this.GetService<ExplorerController>(scope).GetObjectProperty(getPropertyRequest);

                Assert.IsNotNull(getPropertyResponse.ReturnValue);

                var collectionVM = (CollectionVM)getPropertyResponse.ReturnValue;
                Assert.IsFalse(collectionVM.DirtyState.IsDirty);
                Assert.IsFalse(collectionVM.DirtyState.HasDirtyChildren);

                var matchingChild = collectionVM.Items.SingleOrDefault(i => i.ID == _OrderItem.ID);
                Assert.IsNotNull(matchingChild);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}