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
        private GetDomainLibraryCommand _GetDomainLibraryCommand;
        private CreateObjectCommand _CreateObjectCommand;
        private InvokeMethodCommand _InvokeMethodCommand;
        private SearchObjectsCommand _SearchObjectsCommand;

        public ToolboxController
            (
            GetDomainLibraryCommand getDomainLibraryCommand,
            CreateObjectCommand createObjectCommand,
            InvokeMethodCommand invokeMethodCommand,
            SearchObjectsCommand searchObjectsCommand
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
            var results = _GetDomainLibraryCommand.Invoke();
            return results;
        }

        [HttpPost]
        public CreateCommandResponse Create([FromBody]CreateObjectRequest id)
        {
            var request = id;
            var result = _CreateObjectCommand.Invoke(request);
            return result;
        }

        [HttpPost]
        public InvokeMethodResponse InvokeMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var result = _InvokeMethodCommand.Invoke(request);
            return result;
        }

        public SearchResponse SearchObjects(SearchObjectsRequest id)
        {
            var request = id;
            var result = _SearchObjectsCommand.Invoke(request);
            return result;
        }

    }
}