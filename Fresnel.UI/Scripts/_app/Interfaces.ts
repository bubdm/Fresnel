module FresnelApp {

    export interface IToolboxControllerScope extends ng.IScope {
        message: any;

        newInstance: any;

        classHierarchy: any;

        create(fullyQualifiedName: string);

        loadClassHierarchy();
    }

    export interface IExplorerControllerScope extends ng.IScope {
        message: any;
    }

    export interface IIdentityMapControllerScope extends ng.IScope {
        items : any[];

        add(key: string, value: any);

        remove(key: string);

        merge(delta: IdentityMapDelta);
    }
}