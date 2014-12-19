
using Envivo.Fresnel.Utils;
using System.Reflection;


namespace Envivo.Fresnel.Introspection.Templates
{

    public class EnumItemTemplateBuilder
    {
        public EnumItemTemplateBuilder()
        {

        }

        public EnumItemTemplate BuildFor(EnumTemplate tEnum, FieldInfo enumField)
        {
            var result = new EnumItemTemplate(tEnum);
            result.Value = enumField.GetValue(enumField.DeclaringType);

            var enumName = result.Value.ToString();
            result.Name = enumName;
            result.FriendlyName = result.Name.CreateFriendlyName();

            result.FullName = enumField.DeclaringType.DeclaringType != null ?
                              CreateFullNameForNestedEnum(enumField) :
                              CreateDefaultFullName(enumField);

            // If the enumeration label is 'Null' we should return a blank string:
            if (result.Name.IsSameAs("Null_"))
            {
                result.Name = string.Empty;
                result.FriendlyName = string.Empty;
            }

            result.AssemblyReader = tEnum.AssemblyReader;

            return result;
        }

        private string CreateDefaultFullName(FieldInfo enumField)
        {
            return string.Concat(enumField.DeclaringType.Namespace, ".",
                                    enumField.DeclaringType.Name, ".",
                                    enumField.Name);
        }

        private string CreateFullNameForNestedEnum(FieldInfo enumField)
        {
            return string.Concat(enumField.DeclaringType.Namespace, ".",
                                    enumField.DeclaringType.DeclaringType.Name, ".",
                                    enumField.DeclaringType.Name, ".",
                                    enumField.Name);
        }

    }
}
