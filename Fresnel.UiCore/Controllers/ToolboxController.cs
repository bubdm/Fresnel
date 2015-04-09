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
        private CreateObjectCommand _CreateCommand;
        private SearchObjectsCommand _SearchObjectsCommand;

        public ToolboxController
            (
            IEnumerable<ICommand> commands
            )
        {
            _GetClassHierarchyCommand = commands.OfType<GetClassHierarchyCommand>().Single();
            _CreateCommand = commands.OfType<CreateObjectCommand>().Single();
            _SearchObjectsCommand = commands.OfType<SearchObjectsCommand>().Single();
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
            var result = _CreateCommand.Invoke(request);
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