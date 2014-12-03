module FresnelApp {

    export interface IToolboxControllerScope extends ng.IScope {
        message: any;

        newInstance: any;

        classHierarchy: any;

        create(fullyQualifiedName : string);

        loadClassHierarchy();
    }

    export interface IExplorerControllerScope extends ng.IScope {
        message: any;
    }
}