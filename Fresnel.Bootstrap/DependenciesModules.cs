using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.Bootstrap
{
    public class DependenciesModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var type in this.GetSingleInstanceTypes())
            {
                builder.RegisterType(type).SingleInstance();
            }

        }

        private IEnumerable<Type> GetSingleInstanceTypes()
        {
            return new Type[] { 
                typeof(Fresnel.Introspection.Templates.ClassHierarchyBuilder),
                typeof(Fresnel.Introspection.Templates.ClassTemplateBuilder),
                typeof(Fresnel.Introspection.Templates.DynamicMethodBuilder),
                typeof(Fresnel.Introspection.Templates.EnumItemTemplateMapBuilder),
            
            };
        }

    }
}
