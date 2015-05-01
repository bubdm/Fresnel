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
using System.Linq;
using System.Threading;
using TestStack.BDDfy;

namespace Envivo.Fresnel.Tests.Features.Explorer
{
    [TestFixture()]
    public class Add_to_Collection_should_invoke_AddTo_method_if_available
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private ObjectVM _Master;
        private ObjectVM _Child;

        public void Given_the_session_is_already_started()
        {
            _TestScopeContainer = new TestScopeContainer();

            using (var scope = _TestScopeContainer.BeginScope())
            {
                var engine = _TestScopeContainer.Resolve<Core.Engine>();
                engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

                var sessionController = _TestScopeContainer.Resolve<SessionController>();
                _Session = sessionController.GetSession();
            }
        }

        public void When_a_Master_object_is_created()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var createRequest = new CreateObjectRequest()
                {
                    ClassTypeName = typeof(Territory).FullName
                };
                var createResponse = _TestScopeContainer.Resolve<ToolboxController>().Create(createRequest);
                _Master = createResponse.NewObject;
            }
        }

        public void And_when_a_Child_object_is_added_to_a_Collection_property()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var propName = LambdaExtensions.NameOf<Territory>(x => x.Employees);

                var addNewRequest = new CollectionAddNewRequest()
                {
                    ParentObjectID = _Master.ID,
                    CollectionPropertyName = propName,
                    ElementTypeName = typeof(Employee).FullName
                };
                var addResponse = _TestScopeContainer.Resolve<ExplorerController>().AddNewItemToCollection(addNewRequest);
                _Child = addResponse.Modifications.NewObjects.Single(o => o.Type == typeof(Employee).Name &&
                                                                          o.DirtyState.IsTransient);
            }
        }

        public void Then_the_Child_should_have_a_reference_back_to_the_Master()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var propName = LambdaExtensions.NameOf<Employee>(x => x.Territories);

                var getRequest = new GetPropertyRequest
                {
                    ObjectID = _Child.ID,
                    PropertyName = propName
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObjectProperty(getRequest);
                var collectionVM = (CollectionVM)getResponse.ReturnValue;

                Assert.AreEqual(1, collectionVM.Items.Count());
                Assert.AreEqual(_Master.ID, collectionVM.Items.First().ID);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}