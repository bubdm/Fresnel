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

namespace Envivo.Fresnel.UiCore.Editing
{
    public class EditorTypeIdentifier
    {
        private List<IEditorTypeIdentifier> _IdentifierStrategies;

        public EditorTypeIdentifier
            (
            BooleanIdentifier booleanIdentifier,
            DateTimeIdentifier dateTimeIdentifier,
            EnumIdentifier enumIdentifier,
            IntegerIdentifier integerIdentifier,
            NumberIdentifier numberIdentifier,
            TextIdentifier textIdentifier,
            ObjectSelectionIdentifier objectSelectionIdentifier
            )
        {
            _IdentifierStrategies = new List<IEditorTypeIdentifier>()
            {
                booleanIdentifier ,
                dateTimeIdentifier,
                enumIdentifier,
                integerIdentifier,
                numberIdentifier,
                textIdentifier,
                objectSelectionIdentifier,
            };
        }

        public EditorType DetermineEditorFor(BasePropertyObserver oProp)
        {
            var tClass = oProp.Template.InnerClass;

            var valueType = tClass.RealType;
            var actualType = valueType.IsNullableType() ?
                       valueType.GetGenericArguments()[0] :
                       valueType;

            var identifier = _IdentifierStrategies.SingleOrDefault(s => s.CanHandle(oProp, actualType));

            var result = identifier != null ?
                            identifier.DetermineEditorType(oProp, actualType) :
                            EditorType.Unknown;
            return result;
        }

        //private TypeKind GetTypeKind(Type nonReferenceType)
        //{
        //    var type = nonReferenceType.IsNullableType() ?
        //               nonReferenceType.GetGenericArguments()[0] :
        //               nonReferenceType;

        //    if (type.IsEnum)
        //        return TypeKind.Enumeration;

        //    var result = _TypeKindMap.TryGetValueOrDefault(type, TypeKind.Unidentified);
        //    return result;
        //}

        //private EditorType DetermineNonReferenceEditor(NonReferencePropertyObserver oNonRefProp)
        //{
        //    var tClass = oNonRefProp.Template.InnerClass;
        //    var tNonRef = tClass as NonReferenceTemplate;
        //    var typeKind = tNonRef == null ?
        //                    TypeKind.Unidentified :
        //                    tNonRef.KindOf;

        //    switch (tClass.RealType)
        //    {


        //        //case TypeKind.Boolean:
        //        //    {
        //        //        if (tClass.RealType.IsNullableType())
        //        //        {
        //        //            //editor = new NullableBooleanWidget(owner, observer);
        //        //            return EditorType.Boolean;
        //        //        }
        //        //        else
        //        //        {
        //        //            return EditorType.Boolean;
        //        //        }
        //        //    }

        //        case TypeKind.Time:
        //            return EditorType.DateTime;


        //    }

        //    return EditorType.Unknown;
        //}

        //private EditorType DetermineReferenceEditor(ObjectPropertyObserver oObject)
        //{
        //    // Work out which control we need:
        //    var attr = oObject.Template.Attributes.Get<ObjectPropertyAttribute>();

        //    var lookupSpecificationType = attr.LookupListFilter;
        //    if (lookupSpecificationType != null)
        //    {
        //        return EditorType.ObjectSelectionList;
        //    }
        //    else
        //    {
        //        return EditorType.None;
        //    }
        //}

        public EditorType DetermineEditorFor(ParameterTemplate tParam)
        {
            throw new NotImplementedException();
        }
    }
}
