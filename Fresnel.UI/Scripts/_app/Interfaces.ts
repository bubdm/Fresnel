module FresnelApp {

    export interface IToolboxControllerScope extends ng.IScope {
        message: any;
        classHierarchy: any;

        loadClassHierarchy();
    }

    export interface IExplorerControllerScope extends ng.IScope {
        message: any;
    }
}