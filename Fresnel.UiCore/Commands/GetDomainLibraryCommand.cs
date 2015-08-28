using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.UiCore.Model.Classes;
using Envivo.Fresnel.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.DomainTypes.Interfaces;
using Envivo.Fresnel.UiCore.Model.Changes;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetDomainLibraryCommand : ICommand
    {
        private AssemblyReaderMap _AssemblyReaderMap;
        private DomainClassesBuilder _DomainClassesBuilder;
        private DomainServicesBuilder _DomainServicesBuilder;
        private ExceptionMessagesBuilder _ExceptionMessagesBuilder;

        public GetDomainLibraryCommand
            (
            AssemblyReaderMap assemblyReaderMap,
            DomainClassesBuilder domainClassesBuilder,
            DomainServicesBuilder domainServicesBuilder,
            ObserverRetriever observerRetriever,
            ModificationsVmBuilder modificationsBuilder,
            ExceptionMessagesBuilder exceptionMessagesBuilder
            )
        {
            _AssemblyReaderMap = assemblyReaderMap;
            _DomainClassesBuilder = domainClassesBuilder;
            _DomainServicesBuilder = domainServicesBuilder;
            _ExceptionMessagesBuilder = exceptionMessagesBuilder;
        }

        public GetDomainLibraryResponse Invoke()
        {
            try
            {
                var assemblyReader = _AssemblyReaderMap.Values.First(a => !a.IsFrameworkAssembly);

                var result = new GetDomainLibraryResponse();

                var domainClassHierarchy = _DomainClassesBuilder.BuildFor(assemblyReader).ToArray();
                var domainServicesHierarchy = _DomainServicesBuilder.BuildFor(assemblyReader).ToArray();

                return new GetDomainLibraryResponse
                {
                    Passed = true,
                    DomainClasses = domainClassHierarchy,
                    DomainServices = domainServicesHierarchy
                };
            }
            catch (Exception ex)
            {
                var errorVMs = _ExceptionMessagesBuilder.BuildFrom(ex);

                return new GetDomainLibraryResponse()
                {
                    Failed = true,
                    Messages = errorVMs
                };
            }
        }
    }
}