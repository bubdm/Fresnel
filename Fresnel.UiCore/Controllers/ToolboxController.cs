using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.Classes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.UiCore.Objects;

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