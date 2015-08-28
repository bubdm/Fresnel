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
using System.Collections.Generic;

namespace Envivo.Fresnel.Tests.Features.Session
{
    [TestFixture()]
    public class DomainServices_are_identified
    {
        private TestScopeContainer _TestScopeContainer = null;

        private SessionVM _Session;
        private IEnumerable<ServiceClassItem> _ServiceClasses;

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

                var getDomainLibraryResponse = toolboxController.GetDomainLibrary();

                var domainServicesHierarchy = getDomainLibraryResponse.DomainServices;

                _ServiceClasses = domainServicesHierarchy
                                    .SelectMany(ns => ns.Classes)
                                    .Cast<ServiceClassItem>();

                Assert.AreNotEqual(0, _ServiceClasses.Count());
            }
        }

        public void And_Then_all_DomainServices_should_have_bindable_Instances()
        {
            foreach (var serviceClass in _ServiceClasses)
            {
                Assert.IsNotNull(serviceClass.AssociatedService);
            }
        }

        public void And_Then_all_DomainServices_should_be_populated_correctly()
        {
            foreach (var serviceClass in _ServiceClasses)
            {
                Assert.IsNull(serviceClass.Create);
                Assert.IsNull(serviceClass.Search);
                Assert.IsNull(serviceClass.FactoryMethods);
                Assert.IsNull(serviceClass.QueryMethods);
                Assert.AreNotEqual(0, serviceClass.AssociatedService.Methods.Count());
            }
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}