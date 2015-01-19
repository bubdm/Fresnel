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
    public class AbstractPropertyVmBuilder
    {
        private List<IPropertyVmBuilder> _Builders;
        private UnknownVmBuilder _UnknownVmBuilder;

        private PropertyStateVmBuilder _PropertyStateVmBuilder;

        public AbstractPropertyVmBuilder
            (
            BooleanVmBuilder booleanVmBuilder,
            DateTimeVmBuilder dateTimeVmBuilder,
            EnumVmBuilder enumVmBuilder,
            NumberVmBuilder numberVmBuilder,
            StringVmBuilder textVmBuilder,
            ObjectSelectionVmBuilder objectSelectionVmBuilder,
            UnknownVmBuilder unknownVmBuilder,

            PropertyStateVmBuilder propertyStateVmBuilder
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

            _PropertyStateVmBuilder = propertyStateVmBuilder;
        }

        public PropertyVM BuildFor(PropertyTemplate tProp)
        {
            var valueType = tProp.InnerClass.RealType;
            var actualType = valueType.IsNullableType() ?
                               valueType.GetGenericArguments()[0] :
                               valueType;

            var propVM = new PropertyVM()
            {
                Name = tProp.FriendlyName,
                PropertyName = tProp.Name,
                Description = tProp.XmlComments.Summary,
                IsRequired = tProp.IsNonReference && !tProp.IsNullableType,
                IsVisible = !tProp.IsFrameworkMember && tProp.IsVisible,
            };

            var vmBuilder = _Builders.SingleOrDefault(s => s.CanHandle(tProp, actualType)) ?? _UnknownVmBuilder;
            vmBuilder.Populate(propVM, tProp, actualType);

            return propVM;
        }

        public PropertyVM BuildFor(BasePropertyObserver oProp)
        {
            var tProp = oProp.Template;
            var objectProp = oProp as ObjectPropertyObserver;

            var propVM = this.BuildFor(tProp);
            propVM.ObjectID = oProp.OuterObject.ID;
            propVM.IsLoaded = objectProp != null ? objectProp.IsLazyLoaded : true;
            propVM.IsNonReference = oProp.Template.IsNonReference;
            propVM.IsCollection = oProp.Template.IsCollection;
            propVM.IsObject = !propVM.IsNonReference && !propVM.IsCollection;

            propVM.State = _PropertyStateVmBuilder.BuildFor(oProp);

            return propVM;
        }
        
        //private string ConvertToJavascriptType(Type type)
        //{
        //    switch (type.Name.ToLower())
        //    {
        //        case "boolean":
        //            return "boolean";

        //        case "datetime":
        //        case "datetimeoffset":
        //            return "date";

        //        case "decimal":
        //        case "double":
        //        case "single":
        //        case "int32":
        //        case "uint32":
        //        case "int64":
        //        case "uint64":
        //        case "int16":
        //        case "uint16":
        //            return "number";

        //        case "string":
        //        case "char":
        //            return "string";

        //        default:
        //            return "object";
        //    }
        //}
    }
}