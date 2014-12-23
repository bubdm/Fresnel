using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class SessionController : ApiController
    {
        private SessionVmBuilder _SessionVmBuilder;

        public SessionController
            (
            SessionVmBuilder sessionVmBuilder
            )
        {
            _SessionVmBuilder = sessionVmBuilder;
        }

        [HttpGet]
        public SessionVM GetSession()
        {
            return _SessionVmBuilder.Build();
        }

    }
}