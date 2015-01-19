using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public interface IClassConfiguration
    {
        ObjectInstanceAttribute ObjectInstanceConfiguration { get; }

        ObjectConstructorAttribute ConstructorConfiguration { get; }

        IDictionary<string, PropertyAttribute> PropertyConfigurations { get; }

        IDictionary<string, MethodAttribute> MethodConfigurations { get; }

        IDictionary<string, PropertyAttribute> ParameterConfigurations { get; }

        IEnumerable<PermissionsAttribute> ClassPermissions { get; }

        //List<PermissionsAttribute> ConstructorPermissions { get; }
        IDictionary<string, List<PermissionsAttribute>> PropertyPermissions { get; }

        IDictionary<string, List<PermissionsAttribute>> MethodPermissions { get; }

        IDictionary<string, List<PermissionsAttribute>> ParameterPermissions { get; }

        void ConfigureClass(ObjectInstanceAttribute objectInstanceAttribute);

        void ConfigureConstructor(ObjectConstructorAttribute constructorAttribute);

        void ConfigureProperty(string propertyName, PropertyAttribute propertyAttribute);

        void ConfigureMethod(string methodName, MethodAttribute methodAttribute);

        void ConfigureParameter(string methodName, string parameterName, PropertyAttribute parameterAttribute);

        void AddClassPermissions(PermissionsAttribute classPermissions);

        //void AddConstructorPermissions(PermissionsAttribute ctorPermissions);

        void AddPropertyPermissions(string propertyName, PermissionsAttribute propertyPermissions);

        void AddMethodPermissions(string methodName, PermissionsAttribute methodPermissions);

        void AddParameterPermissions(string methodName, string parameterName, PermissionsAttribute parameterPermissions);
    }
}