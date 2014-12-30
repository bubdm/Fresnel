using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ExplorerController : ApiController
    {
        private GetPropertyCommand _GetPropertyCommand;
        private SetPropertyCommand _SetPropertyCommand;
        private InvokeMethodCommand _InvokeMethodCommand;

        public ExplorerController
            (
            GetPropertyCommand getPropertyCommand,
            SetPropertyCommand setPropertyCommand,
            InvokeMethodCommand invokeMethodCommand
            )
        {
            _GetPropertyCommand = getPropertyCommand;
            _SetPropertyCommand = setPropertyCommand;
            _InvokeMethodCommand = invokeMethodCommand;
        }

        [HttpPost]
        public GetPropertyResult GetObjectProperty([FromBody]GetPropertyRequest id)
        {
            var request = id;
            var results = _GetPropertyCommand.Invoke(request);
            return results;
        }

        [HttpPost]
        public SetPropertyResult SetObjectProperty([FromBody]SetPropertyRequest id)
        {
            var request = id;
            var results = _SetPropertyCommand.Invoke(request);
            return results;
        }

        [HttpPost]
        public GetPropertyResult InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var results = _InvokeMethodCommand.Invoke(request);
            return results;
        }
    }
}