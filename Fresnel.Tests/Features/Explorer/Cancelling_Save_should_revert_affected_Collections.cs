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
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using TestStack.BDDfy;

namespace Envivo.Fresnel.Tests.Features.Explorer
{
    [TestFixture()]
    public class Cancelling_Save_should_revert_affected_Collections
    {
        private IContainer _Container;
        private ToolboxController _ToolboxController;
        private ExplorerController _ExplorerController;

        private SessionVM _Session;
        private ObjectVM _ParentObject;
        private ObjectVM _ChildObject;

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

        public void And_given_a_domain_object_is_retrieved()
        {
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(Order).FullName,
                PageSize = 10,
                PageNumber = 1,
            };
            var searchResponse = _ToolboxController.SearchObjects(searchRequest);
            Assert.IsTrue(searchResponse.Passed);
            _ParentObject = searchResponse.Result.Items.First();
        }

        public void When_a_child_object_is_added_to_the_parent()
        {
            var request = new CollectionAddNewRequest()
            {
                ParentObjectID = _ParentObject.ID,
                CollectionPropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
                ElementTypeName = typeof(OrderItem).FullName,
            };
            var createAddResponse = _ExplorerController.AddNewItemToCollection(request);
            Assert.IsTrue(createAddResponse.Passed);

            _ChildObject = createAddResponse.Modifications.NewObjects.Last();
        }

        public void And_when_the_child_object_is_modified()
        {
            var request = new SetPropertyRequest()
            {
                ObjectID = _ChildObject.ID,
                PropertyName = LambdaExtensions.NameOf<OrderItem>(x => x.Quantity),
                NonReferenceValue = 1234
            };
            var setResponse = _ExplorerController.SetProperty(request);
            Assert.IsTrue(setResponse.Passed);
        }

        public void And_when_the_user_Cancels_the_Save()
        {
            var cancelRequest = new CancelChangesRequest()
            {
                ObjectID = _ChildObject.ID,
            };

            var cancelResponse = _ExplorerController.CancelChanges(cancelRequest);
            Assert.IsTrue(cancelResponse.Passed);
        }

        public void Then_the_original_object_should_not_have_dirty_contents()
        {
            var getRequest = new GetObjectRequest()
            {
                ObjectID = _ParentObject.ID
            };
            var getResponse = _ExplorerController.GetObject(getRequest);

            Assert.IsNotNull(getResponse.ReturnValue);

            var getPropertyRequest = new GetPropertyRequest()
            {
                ObjectID = _ParentObject.ID,
                PropertyName = LambdaExtensions.NameOf<Order>(x => x.OrderItems),
            };
            var getPropertyResponse = _ExplorerController.GetObjectProperty(getPropertyRequest);

            Assert.IsNotNull(getPropertyResponse.ReturnValue);

            var collectionVM = (CollectionVM)getPropertyResponse.ReturnValue;
            Assert.IsFalse(collectionVM.DirtyState.IsDirty);
            Assert.IsFalse(collectionVM.DirtyState.HasDirtyChildren);

            var matchingChild = collectionVM.Items.SingleOrDefault(i => i.ID == _ChildObject.ID);
            Assert.IsNull(matchingChild);
        }

        public void And_then_the_child_object_should_not_have_dirty_contents()
        {
            var getRequest = new GetObjectRequest()
            {
                ObjectID = _ChildObject.ID
            };
            var getResponse = _ExplorerController.GetObject(getRequest);

            var prop = getResponse.ReturnValue.Properties.Single(p=> p.InternalName == LambdaExtensions.NameOf<OrderItem>(x => x.Quantity));
            Assert.AreNotEqual(1234, prop.State.Value);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}