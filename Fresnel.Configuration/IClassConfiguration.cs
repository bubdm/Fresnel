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

        IEnumerable<AuthorisationConfiguration> ClassPermissions { get; }

        IDictionary<string, List<AuthorisationConfiguration>> PropertyPermissions { get; }

        IDictionary<string, List<AuthorisationConfiguration>> MethodPermissions { get; }

        IDictionary<string, List<AuthorisationConfiguration>> ParameterPermissions { get; }

        void ConfigureClass(ObjectInstanceConfiguration objectInstanceAttribute);

        void ConfigureConstructor(ObjectConstructorConfiguration constructorAttribute);

        void ConfigureProperty(string propertyName, PropertyConfiguration propertyAttribute);

        void ConfigureMethod(string methodName, MethodConfiguration methodAttribute);

        void ConfigureParameter(string methodName, string parameterName, PropertyConfiguration parameterAttribute);

        void AddClassPermissions(AuthorisationConfiguration classPermissions);

        void AddPropertyPermissions(string propertyName, AuthorisationConfiguration propertyPermissions);

        void AddMethodPermissions(string methodName, AuthorisationConfiguration methodPermissions);

        void AddParameterPermissions(string methodName, string parameterName, AuthorisationConfiguration parameterPermissions);
    }
}