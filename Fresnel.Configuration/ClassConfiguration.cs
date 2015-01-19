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
        private List<PermissionsAttribute> _ClassPermissions;

        public ClassConfiguration()
        {
            _DomainClassName = typeof(T).Name;

            this.PropertyConfigurations = new Dictionary<string, PropertyAttribute>();
            this.MethodConfigurations = new Dictionary<string, MethodAttribute>();
            this.ParameterConfigurations = new Dictionary<string, PropertyAttribute>();

            _ClassPermissions = new List<PermissionsAttribute>();
            //this.ConstructorPermissions = new List<PermissionsAttribute>();
            this.PropertyPermissions = new Dictionary<string, List<PermissionsAttribute>>();
            this.MethodPermissions = new Dictionary<string, List<PermissionsAttribute>>();
            this.ParameterPermissions = new Dictionary<string, List<PermissionsAttribute>>();
        }

        [Browsable(false)]
        public ObjectInstanceAttribute ObjectInstanceConfiguration { get; private set; }

        [Browsable(false)]
        public ObjectConstructorAttribute ConstructorConfiguration { get; private set; }

        [Browsable(false)]
        public IDictionary<string, PropertyAttribute> PropertyConfigurations { get; private set; }

        [Browsable(false)]
        public IDictionary<string, MethodAttribute> MethodConfigurations { get; private set; }

        [Browsable(false)]
        public IDictionary<string, PropertyAttribute> ParameterConfigurations { get; private set; }

        [Browsable(false)]
        public IEnumerable<PermissionsAttribute> ClassPermissions
        {
            get { return _ClassPermissions; }
        }

        //[Browsable(false)]
        //public List<PermissionsAttribute> ConstructorPermissions { get; private set; }

        [Browsable(false)]
        public IDictionary<string, List<PermissionsAttribute>> PropertyPermissions { get; private set; }

        [Browsable(false)]
        public IDictionary<string, List<PermissionsAttribute>> MethodPermissions { get; private set; }

        [Browsable(false)]
        public IDictionary<string, List<PermissionsAttribute>> ParameterPermissions { get; private set; }

        /// <summary>
        /// The class being configured. Use this for identifying class members using LINQ/Lambda expressions.
        /// </summary>
        public T ClassType { get; private set; }

        /// <summary>
        /// Configures this class using the given attribute
        /// </summary>
        /// <param name="objectInstanceAttribute"></param>
        public void ConfigureClass(ObjectInstanceAttribute objectInstanceAttribute)
        {
            if (this.ObjectInstanceConfiguration != null)
            {
                var msg = string.Concat("Cannot have multiple configurations for class ", _DomainClassName);
                throw new ConfigurationException(msg);
            }

            this.ObjectInstanceConfiguration = objectInstanceAttribute;
        }

        /// <summary>
        /// Configures the default Constructor using the given attribute
        /// </summary>
        /// <param name="constructorAttribute"></param>
        public void ConfigureConstructor(ObjectConstructorAttribute constructorAttribute)
        {
            if (this.ConstructorConfiguration != null)
            {
                var msg = string.Concat("Cannot have multiple configurations for constructor on ", _DomainClassName);
                throw new ConfigurationException(msg);
            }

            this.ConstructorConfiguration = constructorAttribute;
        }

        /// <summary>
        /// Configures the Property using the given Attribute
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyAttribute"></param>
        public void ConfigureProperty(string propertyName, PropertyAttribute propertyAttribute)
        {
            var key = propertyName;
            if (this.PropertyConfigurations.Contains(key))
            {
                var msg = string.Concat("Cannot have multiple configurations for property ", _DomainClassName, ".", propertyName);
                throw new ConfigurationException(msg);
            }

            this.PropertyConfigurations.Add(key, propertyAttribute);
        }

        /// <summary>
        /// Configures the Method using the given attribute
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodAttribute"></param>
        public void ConfigureMethod(string methodName, MethodAttribute methodAttribute)
        {
            var key = methodName;
            if (this.MethodConfigurations.Contains(key))
            {
                var msg = string.Concat("Cannot have multiple configurations for method ", _DomainClassName, ".", methodName);
                throw new ConfigurationException(msg);
            }

            this.MethodConfigurations.Add(key, methodAttribute);
        }

        /// <summary>
        /// Configures the Parameter using the given attribute
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterAttribute"></param>
        public void ConfigureParameter(string methodName, string parameterName, PropertyAttribute parameterAttribute)
        {
            var key = string.Concat(methodName, "%", parameterName);
            if (this.ParameterConfigurations.Contains(key))
            {
                var msg = string.Concat("Cannot have multiple configurations for parameter ", _DomainClassName, ".", methodName, "(", parameterName, ")");
                throw new ConfigurationException(msg);
            }

            this.ParameterConfigurations.Add(key, parameterAttribute);
        }

        //-----

        /// <summary>
        /// Adds the given Permission to this Class. NB: multiple Permissions are supported.
        /// </summary>
        /// <param name="classPermissions"></param>
        public void AddClassPermissions(PermissionsAttribute classPermissions)
        {
            _ClassPermissions.Add(classPermissions);
        }

        ///// <summary>
        ///// Adds the given Permission to the default Constructor. NB: multiple Permissions are supported.
        ///// </summary>
        ///// <param name="ctorPermissions"></param>
        //public void AddConstructorPermissions(PermissionsAttribute ctorPermissions)
        //{
        //    this.ConstructorPermissions.Add(ctorPermissions);
        //}

        /// <summary>
        /// Adds the given Permission to the named Property. NB: multiple Permissions are supported.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyPermissions"></param>
        public void AddPropertyPermissions(string propertyName, PermissionsAttribute propertyPermissions)
        {
            var permissionsList = this.PropertyPermissions.TryGetValueOrNull(propertyName);
            if (permissionsList == null)
            {
                permissionsList = new List<PermissionsAttribute>();
                this.PropertyPermissions[propertyName] = permissionsList;
            }
            permissionsList.Add(propertyPermissions);
        }

        /// <summary>
        /// Adds the given Permission to the named Method. NB: multiple Permissions are supported.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodPermissions"></param>
        public void AddMethodPermissions(string methodName, PermissionsAttribute methodPermissions)
        {
            var permissionsList = this.MethodPermissions.TryGetValueOrNull(methodName);
            if (permissionsList == null)
            {
                permissionsList = new List<PermissionsAttribute>();
                this.MethodPermissions[methodName] = permissionsList;
            }
            permissionsList.Add(methodPermissions);
        }

        /// <summary>
        /// Adds the given Permission to the named Method and Parameter. NB: multiple Permissions are supported.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodPermissions"></param>
        public void AddParameterPermissions(string methodName, string parameterName, PermissionsAttribute parameterPermissions)
        {
            var key = string.Concat(methodName, "%", parameterName);
            var permissionsList = this.ParameterPermissions.TryGetValueOrNull(key);
            if (permissionsList == null)
            {
                permissionsList = new List<PermissionsAttribute>();
                this.ParameterPermissions[key] = permissionsList;
            }
            permissionsList.Add(parameterPermissions);
        }
    }
}