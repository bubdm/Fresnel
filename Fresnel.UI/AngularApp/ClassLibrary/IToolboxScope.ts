module FresnelApp {

    export interface IToolboxControllerScope extends ng.IScope {

        classNameFilter: string;

        classHierarchy: Namespace[];

        create(fullyQualifiedName: string);

        searchObjects(fullyQualifiedName: string);

        loadClassHierarchy();

        invokeDependencyMethod(method: MethodVM);
    }

}