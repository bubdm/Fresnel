using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model;
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
        public CleanupSessionResponse CleanUp()
        {
            var result = _CleanupSessionCommand.Invoke();
            return result;
        }
    }
}