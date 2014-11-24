using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;

namespace Envivo.Fresnel.Core.Observers
{

    public class MethodObserverMap : ReadOnlyDictionary<string, MethodObserver>
    {

        public MethodObserverMap(IDictionary<string, MethodObserver> items)
            : base(items)
        {
        }

        public MethodObserver GetMethodThatAccepts(params Type[] argumentTypes)
        {
            // Because we don't have a name, we have to scan by brute force:

            foreach (var oMethod in this.Values)
            {
                var tMethod = oMethod.TemplateAs<MethodTemplate>();

                //var parameterInfos = tMethod.MethodInfo.GetParameters();
                //if (parameterInfos.Length != argumentTypes.Length)
                //    continue;

                //var isMatch = true;
                //for (int p = 0; p < parameterInfos.Length; p++)
                //{
                //    var searchType = argumentTypes[0];
                //    var targetType = parameterInfos[0].ParameterType;
                //    if (!searchType.IsDerivedFrom(targetType))
                //    {
                //        isMatch = false;
                //        break;
                //    }
                //}
                var methodParameterTypes = tMethod.Parameters.Values.Select(p => p.ParameterType).ToArray();
                var isMatch = Enumerable.SequenceEqual(argumentTypes, methodParameterTypes);

                if (isMatch)
                {
                    return oMethod;
                }
            }

            return null;
        }

        ///// <summary>
        ///// Returns the MethodObserver with the given name, and that accepts the given arguments.
        ///// </summary>
        ///// <param name="argumentTypes">The types of the parameters that the method accepts</param>
        
        //internal MethodObserver FindMethod(string methodName, params Type[] argumentTypes)
        //{
        //    if (this.Count == 0)
        //        return null;

        //    var oMethods = new MethodObserver[this.Count];
        //    this.Values.CopyTo(oMethods, 0);

        //    var anyOldMethod = oMethods[0];
        //    var methodInfo = anyOldMethod.Template.RealType.GetMethod(methodName, argumentTypes);

        //    if (methodInfo == null)
        //        return null;

        //    var oMatch = this.TryGetValueOrNull(methodInfo);
        //    return oMatch;
        //}
    }

}
