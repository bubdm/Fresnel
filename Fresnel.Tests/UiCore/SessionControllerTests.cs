using Autofac;
using Envivo.Fresnel.Bootstrap;
using Envivo.Fresnel.UiCore.Controllers;
using NUnit.Framework;
using System;
using System.Threading;

namespace Envivo.Fresnel.Tests.Proxies
{
    [TestFixture()]
    public class SessionControllerTests
    {
        [Test()]
        public void ShouldReturnSingleSession()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var controller = container.Resolve<SessionController>();

            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(SampleModel.IDummy).Assembly);

            var now = DateTime.Now;

            // Act:
            var session1 = controller.GetSession();
            Thread.Sleep(100);

            var session2 = controller.GetSession();

            // Assert:
            Assert.AreEqual(Environment.UserName, session1.UserName);
            Assert.GreaterOrEqual(session1.LogonTime, now);
            Assert.AreEqual(session1.LogonTime, session2.LogonTime);
        }
    }
}