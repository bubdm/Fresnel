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
    public class Method_should_automatically_resolve_Dependency_parameters
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

        public void And_given_an_object_with_a_Double_Dispatch_Method_is_selected()
        {
            var createRequest = new CreateObjectRequest()
            {
                ClassTypeName = typeof(MethodSamples).FullName
            };
            var createResponse = _ToolboxController.Create(createRequest);

            Assert.IsTrue(createResponse.Passed);
            _Object = createResponse.NewObject;
        }

        public void And_given_a_Double_Dispatch_Method_is_selected()
        {
            _DoubleDispatchMethod = LambdaExtensions.NameOf<MethodSamples>(x => x.MethodWithValueParameters(null, null, 0, DateTime.MinValue));
            _Method = _Object.Methods.Single(m => m.InternalName == _DoubleDispatchMethod);
        }

        public void When_the_Method_has_its_normal_parameters_set()
        {
            {
                var setRequest = new SetParameterRequest()
                {
                    ObjectID = _Object.ID,
                    MethodName = _DoubleDispatchMethod,
                    ParameterName = "aString",
                    NonReferenceValue = _Fixture.Create<string>(),
                };
                var setResponse = _ExplorerController.SetParameter(setRequest);
            }

            {
                var setRequest = new SetParameterRequest()
                {
                    ObjectID = _Object.ID,
                    MethodName = _DoubleDispatchMethod,
                    ParameterName = "aNumber",
                    NonReferenceValue = _Fixture.Create<int>(),
                };
                var setResponse = _ExplorerController.SetParameter(setRequest);
            }

            {
                var setRequest = new SetParameterRequest()
                {
                    ObjectID = _Object.ID,
                    MethodName = _DoubleDispatchMethod,
                    ParameterName = "aDate",
                    NonReferenceValue = _Fixture.Create<DateTime>(),
                };
                var setResponse = _ExplorerController.SetParameter(setRequest);
            }
        }

        public void Then_the_Method_should_not_expose_the_Double_Dispatch_parameter()
        {
            var doubleDispatchParam = _Method.Parameters.SingleOrDefault(p => p.InternalName == "enumFilter");
            Assert.IsNull(doubleDispatchParam);
        }

        public void And_then_the_Method_should_execute()
        {
            var invokeRequest = new InvokeMethodRequest()
            {
                ObjectID = _Object.ID,
                MethodName = _DoubleDispatchMethod
            };

            var invokeResponse = _ExplorerController.InvokeMethod(invokeRequest);
            Assert.IsTrue(invokeResponse.Passed);
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

    }
}