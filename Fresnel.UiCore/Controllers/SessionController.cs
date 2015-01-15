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
        private CleanupSessionCommand _CleanupSessionCommand;

        public SessionController
            (
            SessionVmBuilder sessionVmBuilder,
            CleanupSessionCommand cleanupSessionCommand
            )
        {
            _SessionVmBuilder = sessionVmBuilder;
            _CleanupSessionCommand = cleanupSessionCommand;
        }

        [HttpGet]
        public SessionVM GetSession()
        {
            return _SessionVmBuilder.Build();
        }

        [HttpGet]
        public CleanupSessionResult CleanUp()
        {
            var result = _CleanupSessionCommand.Invoke();
            return result;
        }

    }
}