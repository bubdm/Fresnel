using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public class DataTypeAttributeBuilder : IMissingAttributeBuilder
    {
        public bool CanHandle(Type attributeType)
        {
            return attributeType == typeof(DataTypeAttribute);
        }

        public Attribute BuildFrom(Type parentClass, Type templateType, IEnumerable<Attribute> templateAttributes)
        {
            switch (Type.GetTypeCode(templateType))
            {
                case TypeCode.Char:
                case TypeCode.String:
                    return new DataTypeAttribute(DataType.Text);

                case TypeCode.DateTime:
                    return new DataTypeAttribute(DataType.DateTime);
            }

            if (templateType == typeof(TimeSpan))
                return new DataTypeAttribute(DataType.Duration);

            return new DataTypeAttribute("");
        }
    }
}