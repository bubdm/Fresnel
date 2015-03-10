using System;
using System.Linq;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.ComponentModel;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Used to configure a Domain Object's behaviour at run-time
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    public abstract class ClassConfiguration<T> : IClassConfiguration
        where T : class
    {
        private string _DomainClassName = string.Empty;

        private List<Attribute> _ClassAttributes = new List<Attribute>();
        private List<Attribute> _ConstructorAttributes = new List<Attribute>();
        private Dictionary<string, IEnumerable<Attribute>> _PropertyAttributes = new Dictionary<string, IEnumerable<Attribute>>();
        private Dictionary<string, IEnumerable<Attribute>> _MethodAttributes = new Dictionary<string, IEnumerable<Attribute>>();
        private Dictionary<string, IEnumerable<Attribute>> _ParameterAttributes = new Dictionary<string, IEnumerable<Attribute>>();

        private List<Attribute> _ClassPermissions = new List<Attribute>();
        private List<Attribute> _ConstructorPermissions = new List<Attribute>();
        private Dictionary<string, IEnumerable<Attribute>> _PropertyPermissions = new Dictionary<string, IEnumerable<Attribute>>();
        private Dictionary<string, IEnumerable<Attribute>> _MethodPermissions = new Dictionary<string, IEnumerable<Attribute>>();
        private Dictionary<string, IEnumerable<Attribute>> _ParameterPermissions = new Dictionary<string, IEnumerable<Attribute>>();

        public ClassConfiguration()
        {
            _DomainClassName = typeof(T).Name;
        }

        public IEnumerable<Attribute> ClassAttributes
        {
            get { return _ClassAttributes; }
        }

        public IEnumerable<Attribute> ConstructorAttributes
        {
            get { return _ConstructorAttributes; }
        }

        public IDictionary<string, IEnumerable<Attribute>> PropertyAttributes
        {
            get { return _PropertyAttributes; }
        }

        public IDictionary<string, IEnumerable<Attribute>> MethodAttributes
        {
            get { return _MethodAttributes; }
        }

        public IDictionary<string, IEnumerable<Attribute>> ParameterAttributes
        {
            get { return _ParameterAttributes; }
        }

        public IEnumerable<Attribute> ClassPermissions
        {
            get { return _ClassPermissions; }
        }

        public IDictionary<string, IEnumerable<Attribute>> PropertyPermissionAttributes
        {
            get { return _PropertyPermissions; }
        }

        public IDictionary<string, IEnumerable<Attribute>> MethodPermissionAttributes
        {
            get { return _MethodPermissions; }
        }

        public IDictionary<string, IEnumerable<Attribute>> ParameterPermissionAttributes
        {
            get { return _ParameterPermissions; }
        }

        public void ConfigureClass(IEnumerable<Attribute> classAttributes)
        {
            if (_ClassAttributes.Any())
            {
                var msg = string.Concat("Cannot have multiple configurations for class ", _DomainClassName);
                throw new ConfigurationException(msg);
            }

            _ClassAttributes = classAttributes.ToList();
        }

        public void ConfigureConstructor(IEnumerable<Attribute> constructorAttributes)
        {
            if (_ConstructorAttributes.Any())
            {
                var msg = string.Concat("Cannot have multiple configurations for constructor on ", _DomainClassName);
                throw new ConfigurationException(msg);
            }

            _ConstructorAttributes = constructorAttributes.ToList();
        }

        public void ConfigureProperty(string propertyName, IEnumerable<Attribute> propertyAttributes)
        {
            var key = propertyName;
            if (_PropertyAttributes.Contains(key))
            {
                var msg = string.Concat("Cannot have multiple configurations for property ", _DomainClassName, ".", propertyName);
                throw new ConfigurationException(msg);
            }

            _PropertyAttributes.Add(key, propertyAttributes);
        }

        public void ConfigureMethod(string methodName, IEnumerable<Attribute> methodAttributes)
        {
            var key = methodName;
            if (_MethodAttributes.Contains(key))
            {
                var msg = string.Concat("Cannot have multiple configurations for method ", _DomainClassName, ".", methodName);
                throw new ConfigurationException(msg);
            }

            _MethodAttributes.Add(key, methodAttributes);
        }

        public void ConfigureParameter(string methodName, string parameterName, IEnumerable<Attribute> parameterAttributes)
        {
            var key = string.Concat(methodName, "%", parameterName);
            if (_ParameterAttributes.Contains(key))
            {
                var msg = string.Concat("Cannot have multiple configurations for parameter ", _DomainClassName, ".", methodName, "(", parameterName, ")");
                throw new ConfigurationException(msg);
            }

            _ParameterAttributes.Add(key, parameterAttributes);
        }

        public void AddClassPermissions(IEnumerable<Attribute> classPermissions)
        {
            _ClassPermissions.AddRange(classPermissions);
        }

        public void AddPropertyPermissions(string propertyName, IEnumerable<Attribute> propertyPermissionAttributes)
        {
            var permissionsList = _PropertyPermissions.TryGetValueOrNull(propertyName);
            if (permissionsList == null)
            {
                permissionsList = propertyPermissionAttributes;
            }
            else
            {
                var extendedList = permissionsList.ToList();
                extendedList.AddRange(propertyPermissionAttributes);
                permissionsList = extendedList;
            }
            _PropertyPermissions[propertyName] = permissionsList;
        }

        public void AddMethodPermissions(string methodName, IEnumerable<Attribute> methodPermissionAttributes)
        {
            var permissionsList = _MethodPermissions.TryGetValueOrNull(methodName);
            if (permissionsList == null)
            {
                permissionsList = methodPermissionAttributes;
            }
            else
            {
                var extendedList = permissionsList.ToList();
                extendedList.AddRange(methodPermissionAttributes);
                permissionsList = extendedList;
            }
            _PropertyPermissions[methodName] = permissionsList;
        }

        public void AddParameterPermissions(string methodName, string parameterName, IEnumerable<Attribute> parameterPermissionAttributes)
        {
            var key = string.Concat(methodName, "%", parameterName);

            var permissionsList = _ParameterPermissions.TryGetValueOrNull(key);
            if (permissionsList == null)
            {
                permissionsList = parameterPermissionAttributes;
            }
            else
            {
                var extendedList = permissionsList.ToList();
                extendedList.AddRange(parameterPermissionAttributes);
                permissionsList = extendedList;
            }
            _ParameterPermissions[key] = permissionsList;
        }
    }
}