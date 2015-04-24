using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
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
        private ObjectVM _Object;
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

        [Test]
        public void Execute()
        {
            throw new NotImplementedException();
            this.BDDfy();
        }

    }
}