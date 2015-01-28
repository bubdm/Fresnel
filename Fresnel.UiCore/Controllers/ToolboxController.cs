using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Model.Classes;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ToolboxController : ApiController
    {
        private GetClassHierarchyCommand _GetClassHierarchyCommand;
        private CreateCommand _CreateCommand;

        public ToolboxController
            (
            GetClassHierarchyCommand getClassHierarchyCommand,
            CreateCommand createCommand
            )
        {
            _GetClassHierarchyCommand = getClassHierarchyCommand;
            _CreateCommand = createCommand;
        }

        [HttpGet]
        public string GetTestMessage()
        {
            return DateTime.Now.ToString();
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
    }
}