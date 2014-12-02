using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Commands;
using Envivo.Fresnel.UiCore.ClassHierarchy;

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

        public IEnumerable<ClassHierarchyItem> GetClassHierarchy()
        {
            var results = _GetClassHierarchyCommand.Invoke();
            return results;
        }
    }
}