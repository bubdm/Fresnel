using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.TypeInfo;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class AbstractParameterVmBuilder
    {
        private List<IPropertyVmBuilder> _Builders;
        private UnknownVmBuilder _UnknownVmBuilder;


        public AbstractParameterVmBuilder
            (
            BooleanVmBuilder booleanVmBuilder,
            DateTimeVmBuilder dateTimeVmBuilder,
            EnumVmBuilder enumVmBuilder,
            NumberVmBuilder numberVmBuilder,
            StringVmBuilder textVmBuilder,
            ObjectSelectionVmBuilder objectSelectionVmBuilder,
            UnknownVmBuilder unknownVmBuilder
            )
        {
            _Builders = new List<IPropertyVmBuilder>()
            {
                booleanVmBuilder ,
                dateTimeVmBuilder,
                enumVmBuilder,
                numberVmBuilder,
                textVmBuilder,
                objectSelectionVmBuilder,
            };
            _UnknownVmBuilder = unknownVmBuilder;
        }

        public MethodParameterVM BuildFor(ParameterTemplate tParam)
        {
            var valueType = tParam.InnerClass.RealType;
            var actualType = valueType.IsNullableType() ?
                               valueType.GetGenericArguments()[0] :
                               valueType;

            var paramVM = new MethodParameterVM()
            {
                Name = tParam.FriendlyName,
                ParameterName = tParam.Name,
                //Description = tParam.XmlComments.Summary,     // We haven't got parameter XML working yet
                State = new MethodParameterState() { ValueType = tParam.ParameterType.Name }
            };

            var vmBuilder = _Builders.SingleOrDefault(s => s.CanHandle(tParam, actualType)) ?? _UnknownVmBuilder;
            vmBuilder.Populate(paramVM, tParam, actualType);

            return paramVM;
        }

    }
}