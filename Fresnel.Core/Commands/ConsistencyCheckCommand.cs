using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Persistence;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.IoC;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.Core.Commands
{
    public class ConsistencyCheckCommand
    {
        private Type _IConsistencyCheckType = typeof(IConsistencyCheck<>);
        private string _CheckMethodName;

        private IEnumerable<IConsistencyCheck> _ConsistencyChecks;
        private TemplateCache _TemplateCache;

        public ConsistencyCheckCommand
        (
            IEnumerable<IConsistencyCheck> consistencyChecks,
            TemplateCache templateCache
        )
        {
            _ConsistencyChecks = consistencyChecks;
            _TemplateCache = templateCache;

            _CheckMethodName = LambdaExtensions.NameOf<IConsistencyCheck<IEntity>>(x => x.Check(null));
        }

        public ActionResult Check(ObjectObserver oObj)
        {
            var searchType = _IConsistencyCheckType.MakeGenericType(oObj.Template.RealType);
            var consistencyChecker = _ConsistencyChecks.SingleOrDefault(c => c.GetType().IsDerivedFrom(searchType));

            if (consistencyChecker == null)
                // If there's no consistency check, we have to assume everything is OK:
                return ActionResult.Pass;

            var tChecker = (ClassTemplate)_TemplateCache.GetTemplate(consistencyChecker.GetType());
            var tCheckMethod = tChecker.Methods[_CheckMethodName];

            var args = new object[] { oObj.RealObject };
            var failureException = (Exception)tCheckMethod.Invoke(consistencyChecker, args);

            return failureException == null ?
                    ActionResult.Pass :
                    ActionResult.Fail(failureException);
        }

        public ActionResult Check(IEnumerable<ObjectObserver> oObjects)
        {
            var exceptions = new List<Exception>();

            foreach (var oObj in oObjects)
            {
                var searchType = _IConsistencyCheckType.MakeGenericType(oObj.Template.RealType);
                var consistencyChecker = _ConsistencyChecks.SingleOrDefault(c => c.GetType().IsDerivedFrom(searchType));

                if (consistencyChecker == null)
                    // If there's no consistency check, we have to assume everything is OK:
                    return ActionResult.Pass;

                var tChecker = (ClassTemplate)_TemplateCache.GetTemplate(consistencyChecker.GetType());
                var tCheckMethod = tChecker.Methods[_CheckMethodName];

                var args = new object[] { oObj.RealObject };
                var failureException = (Exception)tCheckMethod.Invoke(consistencyChecker, args);

                if (failureException != null)
                {
                    exceptions.AddRange(failureException.FlattenAll());
                }
            }

            return exceptions.Any() ?
                ActionResult.Fail(new AggregateException(exceptions)) :
                ActionResult.Pass;
        }

    }
}