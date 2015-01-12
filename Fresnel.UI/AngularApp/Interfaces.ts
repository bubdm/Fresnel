module FresnelApp {

    export interface IAppControllerScope extends ng.IScope {

        identityMap: IdentityMap;

        session: Session;

        loadSession();
    }

    export interface IToolboxControllerScope extends ng.IScope {

        classHierarchy: any;

        create(fullyQualifiedName: string);

        loadClassHierarchy();

    }

    export interface IObjectExplorerControllerScope extends ng.IScope {

        explorer: Explorer;

        visibleExplorers: Explorer[];

        minimise(explorer: Explorer);

        maximise(explorer: Explorer);

        close(explorer: Explorer);

        invoke(method: IObjectVM);

        setProperty(prop: any);

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