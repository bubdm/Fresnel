module FresnelApp {

    export interface IAppControllerScope extends ng.IScope {

        identityMap: IdentityMap;

        loadSession();
    }

    export interface IToolboxControllerScope extends ng.IScope {

        classHierarchy: any;

        create(fullyQualifiedName: string);

        loadClassHierarchy();

    }

    export interface IObjectExplorerControllerScope extends ng.IScope {

        obj: IObjectVM;

        visibleExplorers: any[];

        minimise(obj: IObjectVM);

        maximise(obj: IObjectVM);

        close(obj: IObjectVM);

        invoke(method: IObjectVM);

        openNewExplorer(prop: any);
    }

    export interface ICollectionExplorerControllerScope extends IObjectExplorerControllerScope {

        gridColumns: any[];

        gridOptions: any;

        addItem(obj: IObjectVM);

        removeItem(obj: IObjectVM);

    }

    export interface IObjectVM {

        ID: string;

        IsCollection: boolean;

        Items: IObjectVM[];

        Properties: any[];

        Methods: any[];

        IsMaximised: boolean;
    }
}