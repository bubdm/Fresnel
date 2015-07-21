using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model.Classes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ToolboxController : ApiController
    {
        private Lazy<GetDomainLibraryCommand> _GetDomainLibraryCommand;
        private Lazy<CreateObjectCommand> _CreateObjectCommand;
        private Lazy<InvokeMethodCommand> _InvokeMethodCommand;
        private Lazy<SearchObjectsCommand> _SearchObjectsCommand;

        public ToolboxController
            (
            Lazy<GetDomainLibraryCommand> getDomainLibraryCommand,
            Lazy<CreateObjectCommand> createObjectCommand,
            Lazy<InvokeMethodCommand> invokeMethodCommand,
            Lazy<SearchObjectsCommand> searchObjectsCommand
            )
        {
            _GetDomainLibraryCommand = getDomainLibraryCommand;
            _CreateObjectCommand = createObjectCommand;
            _InvokeMethodCommand = invokeMethodCommand;
            _SearchObjectsCommand = searchObjectsCommand;
        }

        [HttpGet]
        public GetDomainLibraryResponse GetDomainLibrary()
        {
            var results = _GetDomainLibraryCommand.Value.Invoke();
            return results;
        }

        [HttpPost]
        public CreateCommandResponse Create([FromBody]CreateObjectRequest id)
        {
            var request = id;
            var result = _CreateObjectCommand.Value.Invoke(request);
            return result;
        }

        [HttpPost]
        public InvokeMethodResponse InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var result = _InvokeMethodCommand.Value.Invoke(request);
            return result;
        }

        public SearchResponse SearchObjects(SearchObjectsRequest id)
        {
            var request = id;
            var result = _SearchObjectsCommand.Value.Invoke(request);
            return result;
        }

    }
}