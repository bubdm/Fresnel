using Autofac;
using Envivo.Fresnel.CompositionRoot;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Commands;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.SampleModel.TestTypes;
using Envivo.Fresnel.Utils;
using Fresnel.Tests;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace Envivo.Fresnel.Tests.Domain
{
    [TestFixture()]
    public class MethodTemplateTests
    {
        private Fixture _Fixture = new AutoFixtureFactory().Create();

        [Test]
        public void ShouldInvokeMethod()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var templateCache = container.Resolve<TemplateCache>();
            var methodCommand = container.Resolve<InvokeMethodCommand>();

            var employee = _Fixture.Create<Employee>();

            Assert.AreEqual(0, employee.Notes.Count());

            // Act:
            var methodName = LambdaExtensions.NameOf<Employee>(x => x.AddVacationTime(DateTime.MinValue, DateTime.MinValue));
            var args = new object[] { DateTime.Now.AddDays(10), DateTime.Now.AddDays(15) };
            methodCommand.Invoke(employee, methodName, args);

            // Assert:
            Assert.AreNotEqual(0, employee.Notes.Count());
        }

        [Test]
        public void ShouldReturnValueFromMethod()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);
            
            var templateCache = container.Resolve<TemplateCache>();
            var methodCommand = container.Resolve<InvokeMethodCommand>();

            var obj = container.Resolve<MethodSamples>();

            // Act:
            var methodName = LambdaExtensions.NameOf<MethodSamples>(x => x.MethodThatReturnsA_String());
            var result = methodCommand.Invoke(obj, methodName, null);

            // Assert:
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result);
        }

        [Test]
        public void ShouldInvokeMethodWithArguments()
        {
            // Arrange:
            var container = new ContainerFactory().Build();
            var engine = container.Resolve<Core.Engine>();
            engine.RegisterDomainAssembly(typeof(TextValues).Assembly);

            var templateCache = container.Resolve<TemplateCache>();
            var methodCommand = container.Resolve<InvokeMethodCommand>();

            var obj = container.Resolve<MethodSamples>();

            var classTemplate = (ClassTemplate)templateCache.GetTemplate(obj.GetType());
            var methodName = LambdaExtensions.NameOf<MethodSamples>(x => x.MethodWithOneParameter(DateTime.MinValue));
            var tMethod = classTemplate.Methods[methodName];

            // Act:
            var args = new object[] { DateTime.Now };
            methodCommand.Invoke(obj, tMethod, args);
        }
    }
}