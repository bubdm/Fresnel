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

        public ExplorerController
            (
            GetPropertyCommand getPropertyCommand
            )
        {
            _GetPropertyCommand = getPropertyCommand;
        }

        [HttpPost]
        public GetPropertyResult GetObjectProperty([FromBody]PropertyVM id)
        {
            var prop = id;
            var results = _GetPropertyCommand.Invoke(prop.ObjectID, prop.PropertyName);
            return results;
        }
    }
}