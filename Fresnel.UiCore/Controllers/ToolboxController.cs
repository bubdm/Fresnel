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
        private CreateCommand _CreateCommand;
        private SearchObjectsCommand _SearchObjectsCommand;

        public ToolboxController
            (
            IEnumerable<ICommand> commands
            )
        {
            _GetClassHierarchyCommand = commands.OfType<GetClassHierarchyCommand>().Single();
            _CreateCommand = commands.OfType<CreateCommand>().Single();
            _SearchObjectsCommand = commands.OfType<SearchObjectsCommand>().Single();
        }

        [HttpGet]
        public IEnumerable<Namespace> GetClassHierarchy()
        {
            var results = _GetClassHierarchyCommand.Invoke();
            return results;
        }

        [HttpPost]
        public CreateCommandResponse Create([FromBody]string id)
        {
            var fullyQualifiedName = id;
            var result = _CreateCommand.Invoke(fullyQualifiedName);
            return result;
        }

        public SearchObjectsResponse SearchObjects(SearchObjectsRequest id)
        {
            var fullyQualifiedName = id;
            var result = _SearchObjectsCommand.Invoke(id);
            return result;
        }
    }
}