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
        private InvokeMethodCommand _InvokeMethodCommand;

        public ExplorerController
            (
            GetPropertyCommand getPropertyCommand,
            InvokeMethodCommand invokeMethodCommand
            )
        {
            _GetPropertyCommand = getPropertyCommand;
            _InvokeMethodCommand = invokeMethodCommand;
        }

        [HttpPost]
        public GetPropertyResult GetObjectProperty([FromBody]PropertyGetRequest id)
        {
            var prop = id;
            var results = _GetPropertyCommand.Invoke(prop.ObjectID, prop.PropertyName);
            return results;
        }

        [HttpPost]
        public GetPropertyResult InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var method = id;
            var results = _InvokeMethodCommand.Invoke(method.ObjectID, method.MethodName);
            return results;
        }
    }
}