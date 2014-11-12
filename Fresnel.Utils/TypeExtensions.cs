using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Envivo.Fresnel.Utils
{

    public static class TypeExtensions
    {

        static readonly private Type IEnumerableType = typeof(IEnumerable);

        static readonly internal Type IGenericDictionary = typeof(IDictionary<,>);
        static readonly internal Type IGenericCollection = typeof(ICollection<>);
        static readonly internal Type IGenericEnumerable = typeof(IEnumerable<>);

        static readonly private ICollection<Type> NonReferenceTypes = new List<Type>()
        { 
            typeof(string), 
            typeof(char), 
            typeof(short), 
            typeof(int), 
            typeof(long), 
            typeof(float), 
            typeof(double), 
            typeof(decimal),
            typeof(DateTime), 
            typeof(bool), 
            typeof(byte), 
            typeof(Guid)
        };

        public enum TypeKind
        {
            Unidentified,
            Boolean,
            Integer,
            Floating,
            Enumeration,
            Time,
            Text,
            Guid
        }

        static readonly Dictionary<Type, TypeKind> s_TypeKindMap = CreateTypeKindMap();

        private static Dictionary<Type, TypeKind> CreateTypeKindMap()
        {
            var result = new Dictionary<Type, TypeKind>();

            result.Add(typeof(Int16), TypeKind.Integer);
            result.Add(typeof(Int32), TypeKind.Integer);
            result.Add(typeof(Int64), TypeKind.Integer);
            result.Add(typeof(byte), TypeKind.Integer);

            result.Add(typeof(Guid), TypeKind.Guid);

            result.Add(typeof(double), TypeKind.Floating);
            result.Add(typeof(decimal), TypeKind.Floating);
            result.Add(typeof(float), TypeKind.Floating);

            result.Add(typeof(bool), TypeKind.Boolean);

            result.Add(typeof(DateTime), TypeKind.Time);

            result.Add(typeof(char), TypeKind.Text);
            result.Add(typeof(string), TypeKind.Text);

            return result;
        }

        /// <summary>
        /// Determines if the given type implements IEnumerable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return type.IsDerivedFrom(IEnumerableType);
        }

        /// <summary>
        /// Returns TRUE if the Type of the given Class is considered as a non-reference type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>The value is determined if the Object is neither Complex nor a Collection</remarks>
        public static bool IsNonReference(this Type type)
        {
            // See if we've got a Nullable type:
            if (type.IsGenericType && type.IsValueType)
            {
                type = type.GetGenericArguments()[0];
            }

            if (type.IsPrimitive || type.IsValueType || type.IsEnum || NonReferenceTypes.Contains(type))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns TRUE if the given valueType is Nullable
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNullableType(this Type valueType)
        {
            return valueType.IsGenericType && valueType.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        /// <summary>
        /// Returns TRUE if the SubType can be derived from the SuperType (either a class or an interface)
        /// </summary>
        /// <typeparam name="TSuperType"></typeparam>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static bool IsDerivedFrom<TSuperType>(this Type subType)
        {
            return IsDerivedFrom(subType, typeof(TSuperType));
        }

        /// <summary>
        /// Returns TRUE if the SubType can be derived from the SuperType (either a class or an interface)
        /// </summary>
        /// <param name="subType"></param>
        /// <param name="superType"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsDerivedFrom(this Type subType, Type superType)
        {
            if (subType.IsPrimitive)
                // Primitives can't be subclassed:
                return false;

            if (superType.IsAssignableFrom(subType))
                return true;

            if (superType.IsInterface)
            {
                // Looks like we're searching for an interface with Generic parameters:
                var interfaces = subType.GetInterfaces();
                for (int i = 0; i < interfaces.Length; i++)
                {
                    var iface = interfaces[i];
                    if (iface.IsGenericType == false)
                        continue;

                    if (iface.GetGenericTypeDefinition() != superType)
                        continue;

                    return true;
                }
            }
            else
            {
                // Search the outer class hierarchy for a match:
                var currentType = subType.BaseType;
                while (currentType != null)
                {
                    if (currentType.IsGenericType)
                    {
                        currentType = currentType.GetGenericTypeDefinition();
                    }

                    if (currentType == superType)
                        return true;

                    currentType = currentType.BaseType;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns all of the Classes and Interfaces that this Type inherits from
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetSuperClasses(this Type type)
        {
            var results = new List<Type>();
            results.AddRange(type.GetInterfaces());

            var currentType = type.BaseType;
            while (currentType != null)
            {
                if (currentType.IsGenericType)
                {
                    currentType = currentType.GetGenericTypeDefinition();
                }

                results.Add(currentType);
                results.AddRange(currentType.GetInterfaces());

                currentType = currentType.BaseType;
            }

            return results;
        }

        private static Type GetInterfaceGenericTypeFrom(Type inspectedType, Type expectedInterface)
        {
            var interfaces = inspectedType.GetInterfaces();
            for (int i = 0; i < interfaces.Length; i++)
            {
                var iface = interfaces[i];
                if (!iface.IsGenericType)
                    continue;

                if (iface.GetGenericTypeDefinition() != expectedInterface)
                    continue;

                return iface.GetGenericArguments()[0];
            }

            return null;
        }

        public static Type GetInnerType(this Type genericType)
        {
            if (!genericType.IsGenericType)
                return null;

            return genericType.GetGenericArguments()[0];
        }

        /// <summary>
        /// Collections will return the inner item Type, otherwise it will return the orignal type
        /// </summary>
        /// <remarks></remarks>
        internal static Type DetermineInnerType(this Type originalType)
        {
            var result = typeof(object);

            try
            {
                // Scan up the inheritance chain to find a match:
                var baseType = originalType;
                while ((baseType != null) && (!baseType.IsGenericType))
                {
                    baseType = baseType.BaseType;
                }

                if ((baseType != null) && (baseType.IsGenericType))
                {
                    // It's generic, so we can inspect it:
                    var types = baseType.GetGenericArguments();
                    switch (types.Length)
                    {
                        case 1:
                            // The argument is the Value type
                            result = types[0];
                            break;

                        case 2:
                            // The first argument is the Key type, the second is the Value type
                            result = types[1];
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                // Ignore any exceptions
            }

            return result;
        }

        public static TypeKind GetTypeKind(this Type nonReferenceType)
        {
            var type = nonReferenceType.IsNullableType() ? nonReferenceType.GetGenericArguments()[0] :
                                                           nonReferenceType;

            if (type.IsEnum)
                return TypeKind.Enumeration;

            var result = TypeKind.Unidentified;
            s_TypeKindMap.TryGetValue(type, out result);
            return result;
        }

    }

}
