using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class CollectionRemoveMethodIdentifier
    {
      
        /// <summary>
        /// Returns all of the Methods that are used to remove items from inner collections
        /// </summary>
        /// <param name="tClass"></param>
        /// <returns></returns>
        public IEnumerable<MethodTemplate> GetMethods(ClassTemplate tClass)
        {
            var results = new List<MethodTemplate>();

            foreach (var tProp in tClass.Properties.ForObjects)
            {
                if (!tProp.IsCollection)
                    continue;

                var matchingMethod = this.GetMethod(tProp);
                if (matchingMethod != null)
                {
                    results.Add(matchingMethod);
                }
            }

            return results;
        }


        public MethodTemplate GetMethod(PropertyTemplate tCollectionProp)
        {
            if (!tCollectionProp.IsCollection)
            {
                throw new IntrospectionException("The given Property Template must be a Collection");
            }

            var linkerMethodName = string.Concat("RemoveFrom", tCollectionProp.Name);

            var result = tCollectionProp.OuterClass.Methods.Values
                            .SingleOrDefault(m => m.Name.IsSameAs(linkerMethodName) &&
                                                  m.Parameters.Count == 1);
            return result;
        }
    }
}