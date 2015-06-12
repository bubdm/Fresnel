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
    public class Remove_from_Collection_should_invoke_AddTo_method_if_available
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private ObjectVM _Master;
        private ObjectVM _Child;

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

        public void When_a_Master_object_already_contains_a_child()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var territory = _Fixture.Create<Territory>();
                territory.Employees.Clear();
                var employee = _Fixture.Create<Employee>();
                territory.AddToEmployees(employee);

                Assert.IsTrue(territory.Employees.Contains(employee));
                Assert.IsTrue(employee.Territories.Contains(territory));

                // This ensures the Object is being observed:
                _TestScopeContainer.Resolve<ObserverCache>().GetObserver(territory);
                _TestScopeContainer.Resolve<ObserverCache>().GetObserver(employee);

                _Master = this.GetObjectVM(territory.ID);
                _Child = this.GetObjectVM(employee.ID);
            }
        }

        private ObjectVM GetObjectVM(Guid objectID)
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = objectID,
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getRequest);
                return getResponse.ReturnValue;
            }
        }

        public void And_when_a_Child_object_is_removed_from_the_Collection_property()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var propName = LambdaExtensions.NameOf<Territory>(x => x.Employees);

                var removeRequest = new CollectionRemoveRequest()
                {
                    ParentObjectID = _Master.ID,
                    CollectionPropertyName = propName,
                    ElementID = _Child.ID
                };
                var removeResponse = _TestScopeContainer.Resolve<ExplorerController>().RemoveItemFromCollection(removeRequest);
                Assert.IsTrue(removeResponse.Passed);
            }
        }

        public void Then_the_Child_should_not_have_a_reference_back_to_the_Master()
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

                var match = collectionVM.Items.SingleOrDefault(i => i.ID == _Child.ID);
                Assert.IsNull(match);
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}