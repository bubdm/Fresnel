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
        private GetObjectsCommand _GetObjectsCommand;

        public ToolboxController
            (
            IEnumerable<ICommand> commands
            )
        {
            _GetClassHierarchyCommand = commands.OfType<GetClassHierarchyCommand>().Single();
            _CreateCommand = commands.OfType<CreateCommand>().Single();
            _GetObjectsCommand = commands.OfType<GetObjectsCommand>().Single();
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

        [HttpPost]
        public GetObjectsResponse GetObjects([FromBody]GetObjectsRequest id)
        {
            var fullyQualifiedName = id;
            var result = _GetObjectsCommand.Invoke(id);
            return result;
        }
    }
}