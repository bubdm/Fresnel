module FresnelApp {

    export interface IToolboxControllerScope extends ng.IScope {
        message: any;

        loadNamespaceTree();
    }

    export interface IExplorerControllerScope extends ng.IScope {
        message: any;
    }
}