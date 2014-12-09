using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.Core.Commands;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public class TypeInfoBuilder
    {
        private List<ITypeInfoBuilder> _TypeInfoBuilders;

        private NullVM _NullVM = new NullVM();

        public TypeInfoBuilder
            (
            BooleanVmBuilder booleanVmBuilder,
            DateTimeVmBuilder dateTimeVmBuilder,
            EnumVmBuilder enumVmBuilder,
            NumberVmBuilder numberVmBuilder,
            StringVmBuilder textVmBuilder,
            ObjectSelectionVmBuilder objectSelectionVmBuilder
            )
        {
            _TypeInfoBuilders = new List<ITypeInfoBuilder>()
            {
                booleanVmBuilder ,
                dateTimeVmBuilder,
                enumVmBuilder,
                numberVmBuilder,
                textVmBuilder,
                objectSelectionVmBuilder,
            };
        }

        public ITypeInfo BuildTypeInfoFor(BasePropertyObserver oProp)
        {
            var tClass = oProp.Template.InnerClass;

            var valueType = tClass.RealType;
            var actualType = valueType.IsNullableType() ?
                       valueType.GetGenericArguments()[0] :
                       valueType;

            var builder = _TypeInfoBuilders.SingleOrDefault(s => s.CanHandle(oProp, actualType));

            var result = builder != null ?
                            builder.BuildTypeInfoFor(oProp, actualType) :
                            _NullVM;
            return result;
        }

        public ITypeInfo DetermineEditorFor(ParameterTemplate tParam)
        {
            throw new NotImplementedException();
        }
    }
}
