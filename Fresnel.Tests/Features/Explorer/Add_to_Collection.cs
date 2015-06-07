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
    public class Add_to_Collection
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private ObjectVM _Order;
        private CollectionAddResponse _CollectionAddResponse;

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
                    ClassTypeName = typeof(Order).FullName
                };
                var createResponse = _TestScopeContainer.Resolve<ToolboxController>().Create(createRequest);
                _Order = createResponse.NewObject;
            }
        }

        public void And_when_a_Child_object_is_added_to_a_Collection_property()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var propName = LambdaExtensions.NameOf<Order>(x => x.OrderItems);

                var addNewRequest = new CollectionAddNewRequest()
                {
                    ParentObjectID = _Order.ID,
                    CollectionPropertyName = propName,
                    ElementTypeName = typeof(OrderItem).FullName
                };
                _CollectionAddResponse = _TestScopeContainer.Resolve<ExplorerController>().AddNewItemToCollection(addNewRequest);
            }
        }

        public void Then_the_Response_should_be_correct()
        {
            Assert.IsNotNull(_CollectionAddResponse.AddedItem);
            Assert.AreEqual(typeof(OrderItem).Name, _CollectionAddResponse.AddedItem.Type);

            Assert.AreEqual(1, _CollectionAddResponse.Modifications.CollectionAdditions.Count());
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}