using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.Configuration
{
    public interface IClassConfiguration
    {
        IEnumerable<Attribute> ClassAttributes { get; }

        IEnumerable<Attribute> ConstructorAttributes { get; }

        IDictionary<string, IEnumerable<Attribute>> PropertyAttributes { get; }

        IDictionary<string, IEnumerable<Attribute>> MethodAttributes { get; }

        IDictionary<string, IEnumerable<Attribute>> ParameterAttributes { get; }


        IEnumerable<Attribute> ClassPermissions { get; }

        IDictionary<string, IEnumerable<Attribute>> PropertyPermissionAttributes { get; }

        IDictionary<string, IEnumerable<Attribute>> MethodPermissionAttributes { get; }

        IDictionary<string, IEnumerable<Attribute>> ParameterPermissionAttributes { get; }


        void ConfigureClass(IEnumerable<Attribute> classAttributes);

        void ConfigureConstructor(IEnumerable<Attribute> constructorAttributes);

        void ConfigureProperty(string propertyName, IEnumerable<Attribute> propertyAttributes);

        void ConfigureMethod(string methodName, IEnumerable<Attribute> methodAttributes);

        void ConfigureParameter(string methodName, string parameterName, IEnumerable<Attribute> parameterAttributes);

        void AddClassPermissions(IEnumerable<Attribute> classPermissions);

        void AddPropertyPermissions(string propertyName, IEnumerable<Attribute> propertyPermissionAttributes);

        void AddMethodPermissions(string methodName, IEnumerable<Attribute> methodPermissionAttributes);

        void AddParameterPermissions(string methodName, string parameterName, IEnumerable<Attribute> parameterPermissionAttributes);
    }
}