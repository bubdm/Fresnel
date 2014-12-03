using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.ClassHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class CreateCommand
    {
        private TemplateCache _TemplateCache;
        private CreateObjectCommand _CreateObjectCommand;

        public CreateCommand
            (
            TemplateCache templateCache,
            CreateObjectCommand createObjectCommand
            )
        {
            _TemplateCache = templateCache;
            _CreateObjectCommand = createObjectCommand;
        }

        public object Invoke(string fullyQualifiedName)
        {
            var tClass = _TemplateCache.GetTemplate(fullyQualifiedName);

            if (tClass != null)
            {
                var oObject = _CreateObjectCommand.Invoke(tClass.RealType, null);
                return oObject.RealObject;
            }

            return null;
        }


    }
}
