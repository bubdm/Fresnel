using Envivo.Fresnel.UiCore.Commands;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class DomainServicesController : ApiController
    {
        private CreateAndSetPropertyCommand _CreateAndSetPropertyCommand;
        private GetPropertyCommand _GetPropertyCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private SetParameterCommand _SetParameterCommand;
        private InvokeMethodCommand _InvokeMethodCommand;

        public DomainServicesController
            (
            CreateAndSetPropertyCommand createAndSetPropertyCommand,
            GetPropertyCommand getPropertyCommand,
            SetPropertyCommand setPropertyCommand,
            SetParameterCommand setParameterCommand,
            InvokeMethodCommand invokeMethodCommand
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
            var response = _GetPropertyCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SetProperty([FromBody]SetPropertyRequest id)
        {
            var request = id;
            var response = _SetPropertyCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public BaseCommandResponse CreateAndSetProperty([FromBody]CreateAndSetPropertyRequest id)
        {
            var request = id;
            var response = _CreateAndSetPropertyCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public GenericResponse SetParameter([FromBody]SetParameterRequest id)
        {
            var request = id;
            var response = _SetParameterCommand.Invoke(request);
            return response;
        }

        [HttpPost]
        public InvokeMethodResponse InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var response = _InvokeMethodCommand.Invoke(request);
            return response;
        }
    }
}