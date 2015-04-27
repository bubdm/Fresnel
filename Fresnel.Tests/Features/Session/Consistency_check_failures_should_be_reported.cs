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

namespace Envivo.Fresnel.Tests.Features.Session
{
    [TestFixture()]
    public class Consistency_check_failures_should_be_reported
    {
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private ObjectVM _CreatedObject;
        private SaveChangesResponse _SaveResponse;

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

        public void When_a_domain_object_is_created()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var createRequest = new CreateObjectRequest()
                {
                    ClassTypeName = typeof(Product).FullName
                };
                var createResponse = _TestScopeContainer.Resolve<ToolboxController>().Create(createRequest);
                Assert.IsTrue(createResponse.Passed);

                _CreatedObject = createResponse.NewObject;
            }
        }

        public void And_when_the_invalid_domain_object_is_saved()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var saveRequest = new SaveChangesRequest()
                {
                    ObjectID = _CreatedObject.ID,
                };
                _SaveResponse = _TestScopeContainer.Resolve<ExplorerController>().SaveChanges(saveRequest);
            }
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