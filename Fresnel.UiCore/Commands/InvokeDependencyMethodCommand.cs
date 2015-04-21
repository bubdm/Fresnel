using Envivo.Fresnel.Core;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.UiCore.Model.Changes;
using Envivo.Fresnel.UiCore.Model;

using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Introspection.IoC;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class InvokeDependencyMethodCommand : ICommand
    {
        private TemplateCache _TemplateCache;
        private ObserverCache _ObserverCache;
        private IDomainDependencyResolver _DomainDependencyResolver;
        private InvokeMethodCommand _InvokeMethodCommand;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;
        private IClock _Clock;

        public InvokeDependencyMethodCommand
            (
            TemplateCache templateCache,
            ObserverCache observerCache,
            IDomainDependencyResolver domainDependencyResolver,
            InvokeMethodCommand invokeMethodCommand,
            ExceptionMessagesBuilder exceptionMessagesBuilder,
            IClock clock
        )
        {
            _TemplateCache = templateCache;
            _ObserverCache = observerCache;
            _DomainDependencyResolver = domainDependencyResolver;
            _InvokeMethodCommand = invokeMethodCommand;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
            _Clock = clock;
        }

        public InvokeMethodResponse Invoke(InvokeMethodRequest request)
        {
            try
            {
                var startedAt = SequentialIdGenerator.Next;

                var tClass = (ClassTemplate)_TemplateCache.GetTemplate(request.ClassType);
                if (tClass == null)
                    throw new UiCoreException("Cannot find class template for " + request.ClassType);

                var domainService = _DomainDependencyResolver.Resolve(tClass.RealType);

                var oService = (ObjectObserver)_ObserverCache.GetObserver(domainService);
                var oMethod = oService.Methods[request.MethodName];

                var result = _InvokeMethodCommand.Invoke(request, oMethod, startedAt);
                return result;
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new InvokeMethodResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }

    }
}