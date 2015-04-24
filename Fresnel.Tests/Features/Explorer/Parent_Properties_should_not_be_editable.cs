using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
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
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Threading;
using TestStack.BDDfy;

namespace Envivo.Fresnel.Tests.Features.Explorer
{
    [TestFixture()]
    public class Parent_Properties_should_not_be_editable
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();
        private IContainer _Container;
        private ExplorerController _ExplorerController;
        private ToolboxController _ToolboxController;

        private SessionVM _Session;
        private ObjectVM _ObjectVM;
        private MethodVM _Method;

        private string _DoubleDispatchMethod;

        public void Given_the_session_is_already_started()
        {
            var customDependencyModules = new Autofac.Module[] { new CustomDependencyModule() };
            _Container = new ContainerFactory().Build(customDependencyModules);

            var engine = _Container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            _ExplorerController = _Container.Resolve<ExplorerController>();
            _ToolboxController = _Container.Resolve<ToolboxController>();

            var sessionController = _Container.Resolve<SessionController>();
            _Session = sessionController.GetSession();
        }

        public void When_an_OrderItem_is_retrieved()
        {
            var searchRequest = new SearchObjectsRequest()
            {
                SearchType = typeof(OrderItem).FullName,
                PageSize = 10,
                PageNumber = 1,
            };
            var searchResponse = _ToolboxController.SearchObjects(searchRequest);
            Assert.IsTrue(searchResponse.Passed);
            _ObjectVM = searchResponse.Result.Items.First();
        }

        public void Then_the_Parent_property_should_not_be_modifiable()
        {
            var propName = LambdaExtensions.NameOf<OrderItem>(x => x.ParentOrder);
            var propVM = _ObjectVM.Properties.Single(m => m.InternalName == propName);

            Assert.IsFalse(propVM.State.Set.IsEnabled);
            Assert.IsFalse(propVM.State.Clear.IsEnabled);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}