using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public interface IClassConfiguration
    {
        ObjectInstanceConfiguration ObjectInstanceConfiguration { get; }

        ObjectConstructorConfiguration ConstructorConfiguration { get; }

        IDictionary<string, PropertyConfiguration> PropertyConfigurations { get; }

        IDictionary<string, MethodConfiguration> MethodConfigurations { get; }

        IDictionary<string, PropertyConfiguration> ParameterConfigurations { get; }

        IEnumerable<PermissionsConfiguration> ClassPermissions { get; }

        //List<PermissionsAttribute> ConstructorPermissions { get; }
        IDictionary<string, List<PermissionsConfiguration>> PropertyPermissions { get; }

        IDictionary<string, List<PermissionsConfiguration>> MethodPermissions { get; }

        IDictionary<string, List<PermissionsConfiguration>> ParameterPermissions { get; }

        void ConfigureClass(ObjectInstanceConfiguration objectInstanceAttribute);

        void ConfigureConstructor(ObjectConstructorConfiguration constructorAttribute);

        void ConfigureProperty(string propertyName, PropertyConfiguration propertyAttribute);

        void ConfigureMethod(string methodName, MethodConfiguration methodAttribute);

        void ConfigureParameter(string methodName, string parameterName, PropertyConfiguration parameterAttribute);

        void AddClassPermissions(PermissionsConfiguration classPermissions);

        //void AddConstructorPermissions(PermissionsAttribute ctorPermissions);

        void AddPropertyPermissions(string propertyName, PermissionsConfiguration propertyPermissions);

        void AddMethodPermissions(string methodName, PermissionsConfiguration methodPermissions);

        void AddParameterPermissions(string methodName, string parameterName, PermissionsConfiguration parameterPermissions);
    }
}