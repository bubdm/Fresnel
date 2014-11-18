using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Configuration
{

    public static class ConfigurationTypeExtensions
    {
     
        //static private Type ApplicationConfigurationType = typeof(ApplicationConfiguration);
        static private Type AssemblyConfigurationType = typeof(AssemblyConfiguration<>);
        static private Type ClassConfigurationType = typeof(ClassConfiguration<>);

        ///// <summary>
        ///// Determines if the given type is an Application Configuration
        ///// </summary>
        ///// <param name="type"></param>
        //
        //public static bool IsApplicationConfiguration(this Type type)
        //{
        //    // NB: Checking IsGenericType prevents the actual interface being recognised
        //    return type.IsDerivedFrom(ApplicationConfigurationType) && !type.IsInterface && !type.IsGenericType;
        //}

        /// <summary>
        /// Determines if the given type is an Assembly Configuration/mapper
        /// </summary>
        /// <param name="type"></param>

        public static bool IsAssemblyConfiguration(this Type type)
        {
            // NB: Checking IsGenericType prevents the actual interface being recognised
            return type.IsDerivedFrom(AssemblyConfigurationType) && !type.IsInterface && !type.IsGenericType;
        }

        /// <summary>
        /// Determines if the given type is an Assembly Configuration/mapper
        /// </summary>
        /// <param name="type"></param>

        public static bool IsAssemblyConfiguration(this Type type, out Assembly domainAssembly)
        {
            domainAssembly = null;
            if (type.IsAssemblyConfiguration())
            {
                Type superClass = type.BaseType;
                while (superClass != null && superClass.IsGenericType && superClass.IsDerivedFrom(AssemblyConfigurationType))
                {
                    superClass = superClass.BaseType;
                }
                if (superClass != null)
                {
                    domainAssembly = superClass.GetGenericArguments()[0].Assembly;
                }
            }

            return (domainAssembly != null);
        }

        /// <summary>
        /// Determines if the given type is a class Configuration/mapper
        /// </summary>
        /// <param name="type"></param>

        public static bool IsClassConfiguration(this Type type)
        {
            // NB: Checking IsGenericType prevents the actual interface being recognised
            return type.IsDerivedFrom(ClassConfigurationType) && !type.IsInterface && !type.IsGenericType;
        }

        public static bool IsClassConfiguration(this Type type, out Type tEnum)
        {
            tEnum = null;
            if (type.IsClassConfiguration())
            {
                Type superClass = type.BaseType;
                while (superClass != null && superClass.IsGenericType && superClass.IsDerivedFrom(ClassConfigurationType))
                {
                    superClass = superClass.BaseType;
                }
                if (superClass != null)
                {
                    tEnum = superClass.GetGenericArguments()[0];
                }
            }

            return (tEnum != null);
        }
                
    }
}
