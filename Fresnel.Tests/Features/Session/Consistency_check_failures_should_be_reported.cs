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

namespace Envivo.Fresnel.Tests.Features.Session
{
    [TestFixture()]
    public class Consistency_check_failures_should_be_reported
    {
        private IContainer _Container;
        private ToolboxController _ToolboxController;
        private ExplorerController _ExplorerController;

        private SessionVM _Session;
        private ObjectVM _CreatedObject;
        private SaveChangesResponse _SaveResponse;

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

        public void When_a_domain_object_is_created()
        {
            var createRequest = new CreateObjectRequest()
            {
                ClassTypeName = typeof(Product).FullName
            };
            var createResponse = _ToolboxController.Create(createRequest);
            Assert.IsTrue(createResponse.Passed);

            _CreatedObject = createResponse.NewObject;
        }

        public void And_when_the_invalid_domain_object_is_saved()
        {
            var saveRequest = new SaveChangesRequest()
            {
                ObjectID = _CreatedObject.ID,
            };
            _SaveResponse= _ExplorerController.SaveChanges(saveRequest);
        }
        
        public void Then_the_failures_should_be_reported()
        {
            Assert.IsFalse(_SaveResponse.Passed);
            Assert.AreNotEqual(0, _SaveResponse.Messages.Length);
            Assert.IsTrue(_SaveResponse.Messages.First().Detail.Contains("ValidationException"));
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}