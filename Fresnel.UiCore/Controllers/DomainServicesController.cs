using Envivo.Fresnel.UiCore.Commands;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class DomainServicesController : ApiController
    {
        private Lazy<CreateAndSetPropertyCommand> _CreateAndSetPropertyCommand;
        private Lazy<GetPropertyCommand> _GetPropertyCommand;
        private Lazy<SetPropertyCommand> _SetPropertyCommand;
        private Lazy<SetParameterCommand> _SetParameterCommand;
        private Lazy<InvokeMethodCommand> _InvokeMethodCommand;

        public DomainServicesController
            (
            Lazy<CreateAndSetPropertyCommand> createAndSetPropertyCommand,
            Lazy<GetPropertyCommand> getPropertyCommand,
            Lazy<SetPropertyCommand> setPropertyCommand,
            Lazy<SetParameterCommand> setParameterCommand,
            Lazy<InvokeMethodCommand> invokeMethodCommand
            )
        {
            _CreateAndSetPropertyCommand = createAndSetPropertyCommand;
            _GetPropertyCommand = getPropertyCommand;
            _SetPropertyCommand = setPropertyCommand;
            _SetParameterCommand = setParameterCommand;
            _InvokeMethodCommand = invokeMethodCommand;
        }

        [HttpPost]
        public GetPropertyResponse GetObjectProperty([FromBody]GetPropertyRequest id)
        {
            var request = id;
            var response = _GetPropertyCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SetProperty([FromBody]SetPropertyRequest id)
        {
            var request = id;
            var response = _SetPropertyCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public BaseCommandResponse CreateAndSetProperty([FromBody]CreateAndSetPropertyRequest id)
        {
            var request = id;
            var response = _CreateAndSetPropertyCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SetParameter([FromBody]SetParameterRequest id)
        {
            var request = id;
            var response = _SetParameterCommand.Value.Invoke(request);
            return response;
        }

        [HttpPost]
        public InvokeMethodResponse InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var response = _InvokeMethodCommand.Value.Invoke(request);
            return response;
        }
    }
}