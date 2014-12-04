module FresnelApp {

    export interface IAppControllerScope extends ng.IScope {

        identityMap: IdentityMap;
                
    }

    export interface IToolboxControllerScope extends ng.IScope {

        classHierarchy: any;

        create(fullyQualifiedName: string);

        loadClassHierarchy();

    }

    export interface IExplorerControllerScope extends ng.IScope {

    }
     
}