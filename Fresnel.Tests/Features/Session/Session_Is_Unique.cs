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
    public class Session_is_unique
    {
        private IContainer _Container;

        private DateTime _SessionStartTime;
        private SessionVM _InitialSession;
        private SessionVM _RequestedSession;

        public void Given_the_Session_is_already_started()
        {
            _Container = new ContainerFactory().Build();

            var engine = _Container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            sessionController = _Container.Resolve<SessionController>();
            _SessionStartTime = DateTime.Now;
            _InitialSession = sessionController.GetSession();
        }

        public void when_a_new_Session_is_requested_after_a_short_delay()
        {
            Thread.Sleep(100);

            var controller = _Container.Resolve<SessionController>();
            _RequestedSession = controller.GetSession();
        }

        public void Then_the_same_Session_should_be_reused()
        {
            Assert.AreEqual(Environment.UserName, _InitialSession.UserName);
            Assert.GreaterOrEqual(_InitialSession.LogonTime, _SessionStartTime);
            Assert.AreEqual(_InitialSession.LogonTime, _RequestedSession.LogonTime);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }


        public SessionController sessionController { get; set; }
    }
}