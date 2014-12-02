using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Commands;

namespace Envivo.Fresnel.UiCore.Controllers
{
    public class ToolboxController : ApiController
    {
        private GetClassHierarchyCommand _GetClassHierarchyCommand;

        public ToolboxController
            (
            GetClassHierarchyCommand getClassHierarchyCommand
            )
        {
            _GetClassHierarchyCommand = getClassHierarchyCommand;
        }

        public string GetTestMessage()
        {
            return DateTime.Now.ToString();
        }

        public HierarchyNode GetNamespaceTree()
        {
            var result = _GetClassHierarchyCommand.Invoke();
            return result;
        }
    }
}