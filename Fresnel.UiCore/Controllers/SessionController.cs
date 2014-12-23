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

        private Lazy<SessionVM> _SessionVM;

        public SessionController
            (
            SessionVmBuilder sessionVmBuilder
            )
        {
            _SessionVmBuilder = sessionVmBuilder;

            _SessionVM = new Lazy<SessionVM>(
                                () => _SessionVmBuilder.Build(), 
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        [HttpGet]
        public SessionVM GetSession()
        {
            return _SessionVM.Value;
        }

    }
}