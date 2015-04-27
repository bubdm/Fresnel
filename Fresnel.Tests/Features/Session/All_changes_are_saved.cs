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
    public class All_changes_are_saved
    {
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private ObjectVM _CreatedObject;

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
                        ClassTypeName = typeof(MultiType).FullName
                    };
                var createResponse = _TestScopeContainer.Resolve<ToolboxController>().Create(createRequest);

                Assert.IsTrue(createResponse.Passed);

                _CreatedObject = createResponse.NewObject;
            }
        }

        public void And_when_the_domain_object_is_modified()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var propName = LambdaExtensions.NameOf<MultiType>(x => x.A_String);
                var prop = _CreatedObject.Properties.Single(p => p.InternalName == propName);
                prop.State.Value = DateTime.Now.ToString();

                var request = new SetPropertyRequest()
                {
                    ObjectID = _CreatedObject.ID,
                    PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_String),
                    NonReferenceValue = prop.State.Value
                };
                var setResponse = _TestScopeContainer.Resolve<ExplorerController>().SetProperty(request);
                Assert.IsTrue(setResponse.Passed);
            }
        }

        public void And_when_the_user_Saves_all_changes()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var saveRequest = new SaveChangesRequest()
                    {
                        ObjectID = _CreatedObject.ID,
                    };

                var saveResponse = _TestScopeContainer.Resolve<ExplorerController>().SaveChanges(saveRequest);
                Assert.IsTrue(saveResponse.Passed);
            }
        }

        public void Then_the_saved_object_should_match_the_domain_object()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var getRequest = new GetObjectRequest()
                {
                    ObjectID = _CreatedObject.ID
                };
                var getResponse = _TestScopeContainer.Resolve<ExplorerController>().GetObject(getRequest);

                Assert.IsNotNull(getResponse.ReturnValue);

                var persistedObj = getResponse.ReturnValue;
                var knownValues = _CreatedObject.Properties
                                    .Select(p => p.State.Value.ToStringOrNull())
                                    .ToArray();

                var latestValues = persistedObj.Properties
                                    .Select(p => p.State.Value.ToStringOrNull())
                                    .ToArray();

                for (var i = 0; i < knownValues.Length; i++)
                {
                    Assert.AreEqual(knownValues[i], latestValues[i]);
                }
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}