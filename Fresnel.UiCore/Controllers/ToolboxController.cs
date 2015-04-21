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
        private GetClassHierarchyCommand _GetClassHierarchyCommand;
        private CreateObjectCommand _CreateObjectCommand;
        private InvokeDependencyMethodCommand _InvokeDependencyMethodCommand;
        private SearchObjectsCommand _SearchObjectsCommand;

        public ToolboxController
            (
            GetClassHierarchyCommand getClassHierarchyCommand,
            CreateObjectCommand createObjectCommand,
            InvokeDependencyMethodCommand invokeServiceMethodCommand,
            SearchObjectsCommand searchObjectsCommand
            )
        {
            _GetClassHierarchyCommand = getClassHierarchyCommand;
            _CreateObjectCommand = createObjectCommand;
            _InvokeDependencyMethodCommand = invokeServiceMethodCommand;
            _SearchObjectsCommand = searchObjectsCommand;
        }

        [HttpGet]
        public IEnumerable<Namespace> GetClassHierarchy()
        {
            var results = _GetClassHierarchyCommand.Invoke();
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
        public InvokeMethodResponse InvokeDependencyMethod([FromBody]InvokeMethodRequest id)
        {
            var request = id;
            var result = _InvokeDependencyMethodCommand.Invoke(request);
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