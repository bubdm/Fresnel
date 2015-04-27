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
    public class Cleaning_Session_should_remove_old_references
    {
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private ObjectVM _Object;

        public void Given_the_Session_is_already_started()
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

        public void when_a_domain_object_is_retrieved()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var searchRequest = new SearchObjectsRequest()
                {
                    SearchType = typeof(MultiType).FullName,
                    PageSize = 10,
                    PageNumber = 1,
                };
                var searchResponse = _TestScopeContainer.Resolve<ToolboxController>().SearchObjects(searchRequest);
                Assert.IsTrue(searchResponse.Passed);
                _Object = searchResponse.Result.Items.First();
            }
        }

        public void And_when_the_domain_object_is_modified()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var request = new SetPropertyRequest()
                    {
                        ObjectID = _Object.ID,
                        PropertyName = LambdaExtensions.NameOf<MultiType>(x => x.A_DateTime),
                        NonReferenceValue = DateTime.Now.ToString()
                    };
                var setResponse = _TestScopeContainer.Resolve<ExplorerController>().SetProperty(request);
                Assert.IsTrue(setResponse.Passed);
            }
        }

        public void And_when_the_Session_is_cleared_without_saving_changes()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                _TestScopeContainer.Resolve<SessionController>().CleanUp();
            }
        }

        public void Then_the_changes_should_be_lost()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var searchRequest = new SearchObjectsRequest()
                    {
                        SearchType = typeof(MultiType).FullName,
                        PageSize = 10,
                        PageNumber = 1,
                    };
                var searchResponse = _TestScopeContainer.Resolve<ToolboxController>().SearchObjects(searchRequest);
                var persistedObj = searchResponse.Result.Items.First();

                var originalValues = _Object.Properties
                                    .Select(p => p.State.Value.ToStringOrNull())
                                    .ToArray();

                var latestValues = persistedObj.Properties
                                    .Select(p => p.State.Value.ToStringOrNull())
                                    .ToArray();

                for (var i = 0; i < originalValues.Length; i++)
                {
                    Assert.AreEqual(originalValues[i], latestValues[i]);
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