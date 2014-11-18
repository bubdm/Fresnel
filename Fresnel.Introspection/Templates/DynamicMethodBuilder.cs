using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Envivo.Fresnel.Introspection.Templates
{
    public delegate object RapidCtor();
    public delegate object RapidGet(object target);
    public delegate void RapidSet(object target, object value);
    public delegate object RapidMethod(object target, object[] parameters);
    
    /// <summary>
    /// This class has been cobbled together from various sources:
    ///     http://www.codeproject.com/useritems/Dynamic_Code_Generation.asp
    ///     http://jachman.wordpress.com/2006/08/22/2000-faster-using-dynamic-method-calls/
    /// </summary>
    public class DynamicMethodBuilder
    {
        private readonly Type _objectType = typeof(object);
        private readonly Type _nullType = typeof(void);

        /// <summary>
        /// Returns a delegate to handle instantiation of the given Class
        /// </summary>
        /// <param name="type"></param>
        
        public RapidCtor BuildCreateObjectHandler(Type type)
        {
            var ctorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                               null,
                               new Type[0],
                               null);

            if (ctorInfo == null)
            {
                var msg = string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", type);
                throw new ApplicationException(msg);
            }

            var dynamicMethodName = string.Concat("Create", type.Name);
            var dynamicCtor = new DynamicMethod(dynamicMethodName, type, null, type.Module, true);

            var ilGen = dynamicCtor.GetILGenerator();
            ilGen.Emit(OpCodes.Newobj, ctorInfo);
            ilGen.Emit(OpCodes.Ret);
            return (RapidCtor)dynamicCtor.CreateDelegate(typeof(RapidCtor));
        }

        /// <summary>
        /// Returns TRUE if the given MethodInfo uses Generics in any way
        /// </summary>
        /// <param name="method"></param>
        
        private bool UsesGenerics(MethodInfo method)
        {
            if (method.ContainsGenericParameters)
                return true;

            if (method.IsGenericMethod)
                return true;

            if (method.IsGenericMethodDefinition)
                return true;

            return false;
        }

        /// <summary>
        /// Returns a delegate to handle value retrieval from the given field
        /// </summary>
        /// <param name="fieldInfo"></param>
        
        public RapidGet BuildGetHandler(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                return null;

            var parameterTypes = new Type[] { _objectType };
            var dynamicMethodName = string.Concat("GetField", fieldInfo.Name);
            var dynamicGet = new DynamicMethod(dynamicMethodName, _objectType, parameterTypes, fieldInfo.Module, true);
            var ilGen = dynamicGet.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, fieldInfo);
            EmitBoxIfNeeded(fieldInfo.FieldType, ilGen);
            ilGen.Emit(OpCodes.Ret);

            return (RapidGet)dynamicGet.CreateDelegate(typeof(RapidGet));
        }

        /// <summary>
        /// Returns a delegate to handle setting the given field
        /// </summary>
        /// <param name="fieldInfo"></param>
        
        public RapidSet BuildSetHandler(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                return null;

            var parameterTypes = new Type[] { _objectType, _objectType };
            var dynamicMethodName = string.Concat("SetField", fieldInfo.Name);
            var dynamicSet = new DynamicMethod(dynamicMethodName, _nullType, parameterTypes, fieldInfo.Module, true);
            var ilGen = dynamicSet.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldarg_1);
            EmitUnboxIfNeeded(fieldInfo.FieldType, ilGen);
            ilGen.Emit(OpCodes.Stfld, fieldInfo);
            ilGen.Emit(OpCodes.Ret);

            return (RapidSet)dynamicSet.CreateDelegate(typeof(RapidSet));
        }

        /// <summary>
        /// Returns a delegate to handle value retrieval from the given Property
        /// </summary>
        /// <param name="propertyInfo"></param>
        
        public RapidGet BuildGetHandler(PropertyInfo propertyInfo)
        {
            var getInfo = propertyInfo.GetGetMethod(true);

            if (getInfo == null)
                // This must be a write-only property:
                return null;

            // We don't know how to handle Generics yet:
            if (UsesGenerics(getInfo))
                return null;

            var parameterTypes = new Type[] { _objectType };
            var dynamicMethodName = string.Concat("Get", propertyInfo.Name);
            var dynamicGet = new DynamicMethod(dynamicMethodName, _objectType, parameterTypes, propertyInfo.Module, true);
            var ilGen = dynamicGet.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.EmitCall(OpCodes.Callvirt, getInfo, null);

            EmitBoxIfNeeded(propertyInfo.PropertyType, ilGen);

            ilGen.Emit(OpCodes.Ret);

            return (RapidGet)dynamicGet.CreateDelegate(typeof(RapidGet));
        }

        /// <summary>
        /// Returns a delegate to handle setting the given Property
        /// </summary>
        /// <param name="propertyInfo"></param>
        
        public RapidSet BuildSetHandler(PropertyInfo propertyInfo)
        {
            var setInfo = propertyInfo.GetSetMethod(true);

            if (setInfo == null)
                // This must be a read-only property:
                return null;

            // We don't know how to handle Generics yet:
            if (UsesGenerics(setInfo))
                return null;

            var parameterTypes = new Type[] { _objectType, _objectType };
            var dynamicMethodName = string.Concat("Set", propertyInfo.Name);
            var dynamicSet = new DynamicMethod(dynamicMethodName, _nullType, parameterTypes, propertyInfo.Module, true);
            var ilGen = dynamicSet.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldarg_1);

            if (propertyInfo.PropertyType.IsValueType)
            {
                ilGen.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            }
            else
            {
                ilGen.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            }

            ilGen.EmitCall(OpCodes.Callvirt, setInfo, null);
            ilGen.Emit(OpCodes.Ret);

            return (RapidSet)dynamicSet.CreateDelegate(typeof(RapidSet));
        }

        public RapidMethod BuildMethodHandler(MethodInfo methodInfo)
        {
            // We don't know how to handle Generics yet:
            if (UsesGenerics(methodInfo))
                return null;

            var dynamicMethodName = string.Concat("Invoke", methodInfo.Name);
            var dynamicMethod = new DynamicMethod(dynamicMethodName, _objectType, new Type[] { _objectType, typeof(object[]) }, methodInfo.Module, true);
            var ilGen = dynamicMethod.GetILGenerator();

            var parameterInfos = methodInfo.GetParameters();
            var parameterTypes = new Type[parameterInfos.Length];
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterInfos[i].ParameterType.IsByRef)
                {
                    parameterTypes[i] = parameterInfos[i].ParameterType.GetElementType();
                }
                else
                {
                    parameterTypes[i] = parameterInfos[i].ParameterType;
                }
            }

            var localBuilders = new LocalBuilder[parameterTypes.Length];
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                localBuilders[i] = ilGen.DeclareLocal(parameterTypes[i], true);
            }

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                ilGen.Emit(OpCodes.Ldarg_1);
                EmitFastInt(i, ilGen);
                ilGen.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(parameterTypes[i], ilGen);
                ilGen.Emit(OpCodes.Stloc, localBuilders[i]);
            }

            if (methodInfo.IsStatic == false)
            {
                ilGen.Emit(OpCodes.Ldarg_0);
            }

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterInfos[i].ParameterType.IsByRef)
                {
                    ilGen.Emit(OpCodes.Ldloca_S, localBuilders[i]);
                }
                else
                {
                    ilGen.Emit(OpCodes.Ldloc, localBuilders[i]);
                }
            }

            if (methodInfo.IsStatic)
            {
                ilGen.EmitCall(OpCodes.Call, methodInfo, null);
            }
            else
            {
                ilGen.EmitCall(OpCodes.Callvirt, methodInfo, null);
            }

            if (methodInfo.ReturnType == typeof(void))
            {
                ilGen.Emit(OpCodes.Ldnull);
            }
            else
            {
                EmitBoxIfNeeded(methodInfo.ReturnType, ilGen);
            }

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterInfos[i].ParameterType.IsByRef)
                {
                    ilGen.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(i, ilGen);
                    ilGen.Emit(OpCodes.Ldloc, localBuilders[i]);
                    if (localBuilders[i].LocalType.IsValueType)
                    {
                        ilGen.Emit(OpCodes.Box, localBuilders[i].LocalType);
                    }
                    ilGen.Emit(OpCodes.Stelem_Ref);
                }
            }

            ilGen.Emit(OpCodes.Ret);

            return (RapidMethod)dynamicMethod.CreateDelegate(typeof(RapidMethod));
        }

        private void EmitBoxIfNeeded(Type type, ILGenerator ilGen)
        {
            if (type.IsValueType)
            {
                ilGen.Emit(OpCodes.Box, type);
            }
        }

        private void EmitUnboxIfNeeded(Type type, ILGenerator ilGen)
        {
            if (type.IsValueType)
            {
                ilGen.Emit(OpCodes.Unbox_Any, type);
            }
        }

        private void EmitCastToReference(Type type, ILGenerator ilGen)
        {
            if (type.IsValueType)
            {
                ilGen.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                ilGen.Emit(OpCodes.Castclass, type);
            }
        }

        private void EmitFastInt(int value, ILGenerator ilGen)
        {
            switch (value)
            {
                case -1:
                    ilGen.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    ilGen.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    ilGen.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    ilGen.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    ilGen.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    ilGen.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    ilGen.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    ilGen.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    ilGen.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    ilGen.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                ilGen.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                ilGen.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
}
