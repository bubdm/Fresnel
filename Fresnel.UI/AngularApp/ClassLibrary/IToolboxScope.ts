module FresnelApp {

    export interface IToolboxControllerScope extends ng.IScope {

        classNameFilter: string;

        domainClassesHierarchy: Namespace[];

        domainServicesHierarchy: Namespace[];

        create(fullyQualifiedName: string);

        searchObjects(fullyQualifiedName: string);

        loadDomainLibrary();

        invokeDependencyMethod(method: MethodVM);
    }

}