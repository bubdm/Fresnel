using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class SessionController : ApiController
    {
        private Lazy<SessionVmBuilder> _SessionVmBuilder;
        private Lazy<CleanupSessionCommand> _CleanupSessionCommand;

        public SessionController
            (
            Lazy<SessionVmBuilder> sessionVmBuilder,
            Lazy<CleanupSessionCommand> cleanupSessionCommand
            )
        {
            _SessionVmBuilder = sessionVmBuilder;
            _CleanupSessionCommand = cleanupSessionCommand;
        }

        [HttpGet]
        public SessionVM GetSession()
        {
            return _SessionVmBuilder.Value.Build();
        }

        [HttpGet]
        public GenericResponse CleanUp()
        {
            var result = _CleanupSessionCommand.Value.Invoke();
            return result;
        }
    }
}