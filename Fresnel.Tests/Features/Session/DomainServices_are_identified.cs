using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Controllers;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.Classes;
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
    public class DomainServices_are_identified
    {
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private Namespace _Namespace;

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

        public void Then_all_DomainServices_should_be_returned()
        {
            using (var scope = _TestScopeContainer.BeginScope())
            {
                var toolboxController = _TestScopeContainer.Resolve<ToolboxController>();

                var getDomainServicesResponse = toolboxController.GetDomainServicesHierarchy();

                Assert.AreNotEqual(0, getDomainServicesResponse.Namespaces.Count());

                var serviceClasses = getDomainServicesResponse.Namespaces.SelectMany(ns => ns.Classes);

                foreach (var serviceClass in serviceClasses)
                {
                    Assert.IsNull(serviceClass.Create);
                    Assert.IsNull(serviceClass.Search);
                    Assert.IsNull(serviceClass.FactoryMethods);
                    Assert.IsNull(serviceClass.QueryMethods);
                    Assert.AreNotEqual(0, serviceClass.ServiceMethods.Count());
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