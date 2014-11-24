using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Proxies;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envivo.Fresnel.Proxies
{

    /// <summary>
    /// Determines if the given object can be converted to an IFresnelProxy
    /// </summary>
    public class CanBeProxiedSpecification : ISpecification<object>
    {
        private readonly RealTypeResolver _RealTypeResolver;

        private readonly TemplateCache _TemplateCache;
        private readonly Dictionary<Type, string> _ValidViewModelTypes = new Dictionary<Type, string>();
        private readonly Dictionary<Type, string> _InvalidViewModelTypes = new Dictionary<Type, string>();

        private const BindingFlags _BindingFlags = BindingFlags.Public |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.Instance |
                                                   BindingFlags.FlattenHierarchy;

        private readonly Type _ObjectType = typeof(object);

        public CanBeProxiedSpecification
            (
            TemplateCache templateCache,
            RealTypeResolver realTypeResolver
            )
        {
            _TemplateCache = templateCache;
            _RealTypeResolver = realTypeResolver;
        }

        public IAssertion IsSatisfiedBy(object obj)
        {
            if (obj == null)
                return Assertion.Fail("Object is null");

            var type = _RealTypeResolver.GetRealType(obj.GetType());

            //-----

            if (_ValidViewModelTypes.Contains(type))
            {
                return Assertion.Pass();
            }

            if (_InvalidViewModelTypes.Contains(type))
            {
                return Assertion.Fail(_InvalidViewModelTypes[type]);
            }

            //-----

            string failureReason = null;
            if (obj is IFresnelProxy)
            {
                failureReason = string.Concat("'", type.Name, "' is already an IFresnelProxy");
            }
            else if (obj.GetType().IsNonReference())
            {
                failureReason = string.Concat("'", type.Name, "' is a non-reference object");
            }
            else if (obj is BaseObserver)
            {
                failureReason = string.Concat("'", type.Name, "' cannot be converted to IFresnelProxy");
            }
            else if (obj is MulticastDelegate)
            {
                failureReason = string.Concat("'", type.Name, "' cannot be converted to IFresnelProxy");
            }
            else if (obj is IEnumerator)
            {
                failureReason = string.Concat("'", type.Name, "' cannot be converted to IFresnelProxy");
            }

            if (failureReason.IsEmpty())
            {
                var virtualMemberCheck = this.ContainsVirtualMembers(type);
                if (virtualMemberCheck.Failed)
                {
                    failureReason = virtualMemberCheck.FailureReason;
                }
            }

            if (failureReason.IsEmpty())
            {
                var collectionPropertiesCheck = this.CheckCollectionPropertiesHaveCorrectType(type);
                if (collectionPropertiesCheck.Failed)
                {
                    failureReason = collectionPropertiesCheck.FailureReason;
                }
            }

            //-----

            if (failureReason.IsNotEmpty())
            {
                _InvalidViewModelTypes.Add(type, failureReason);
                return Assertion.Fail(failureReason);
            }
            else
            {
                _ValidViewModelTypes.Add(type, null);
                return Assertion.Pass();
            }
        }

        private IAssertion ContainsVirtualMembers(Type objectType)
        {
            var virtualMembers = new List<MethodInfo>();
            var objectMembers = new List<MethodInfo>();
            var nonVirtualMembers = new List<MethodInfo>();

            foreach (var method in objectType.GetMethods(_BindingFlags))
            {
                if (method.IsPrivate)
                    continue;

                var isDefinedOnSystemObject = method.DeclaringType == _ObjectType;
                if (isDefinedOnSystemObject == false)
                {
                    var baseDeclaringType = method.GetBaseDefinition().DeclaringType;
                    isDefinedOnSystemObject = (baseDeclaringType != null && baseDeclaringType == _ObjectType);
                }

                //-----

                if (isDefinedOnSystemObject)
                {
                    objectMembers.Add(method);
                }
                else if (method.IsVirtual)
                {
                    virtualMembers.Add(method);
                }
                else
                {
                    nonVirtualMembers.Add(method);
                }
            }

            //-----

            if (virtualMembers.Count == 0)
            {
                return Assertion.Fail(string.Concat("'", objectType.Name, " doesn't have any virtual methods"));
            }

            if (nonVirtualMembers.Count > 0)
            {
                var msg = string.Concat("'", objectType.Name, " must ONLY contain virtual methods : ",
                                                    string.Join(", ", nonVirtualMembers.Select(p => p.Name)));
                return Assertion.Fail(msg);
            }

            return Assertion.Pass();
        }


        private IAssertion CheckCollectionPropertiesHaveCorrectType(Type objectType)
        {
            // All Collection properties must be interface (otherwise Castle won't intercept the methods!) 
            var tClass = _TemplateCache.GetTemplate(objectType) as ClassTemplate;

            var invalidPropertyNames = new List<string>();
            foreach (var tProp in tClass.Properties.ForObjects)
            {
                if (tProp.IsCollection && tProp.PropertyType.IsInterface == false)
                {
                    invalidPropertyNames.Add(tProp.Name);
                }
            }

            if (invalidPropertyNames.Count == 0)
            {
                return Assertion.Pass();
            }
            else
            {
                var msg = string.Concat("'", objectType.Name,
                                        "' MUST have Collection properties that return Interfaces (not concrete types): ",
                                        string.Join(", ", invalidPropertyNames.ToArray()));
                return Assertion.Fail(msg);
            }
        }

    }

}
