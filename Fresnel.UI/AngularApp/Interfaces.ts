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

    export interface IWorkbenchControllerScope extends ng.IScope {

        visibleExplorers: Explorer[];
    }

    export interface IExplorerControllerScope extends ng.IScope {

        explorer: Explorer;

        minimise(explorer: Explorer);

        maximise(explorer: Explorer);

        close(explorer: Explorer);

        refresh(explorer: Explorer);

        invoke(method: IObjectVM);

        setProperty(prop: any);

        setBitwiseEnumProperty(prop: any, enumValue: number);

        openNewExplorer(obj: IObjectVM);

        openNewExplorerForProperty(prop: any);

    }

    export interface ICollectionExplorerControllerScope extends IExplorerControllerScope {

        gridColumns: any[];

        gridOptions: any;

        addNewItem(itemType: string);

        addExistingItem(obj: IObjectVM);

        removeItem(obj: IObjectVM);

    }

    export interface IMethodControllerScope extends ng.IScope {

        explorer: Explorer;

        method: any;

        invoke(method: any);

        setProperty(param: any);

        setBitwiseEnumProperty(param: any, enumValue: number);

        cancel();
    }

    export interface ISearchControllerScope extends ng.IScope {

        explorer: Explorer;

        searchType: string;

        close(explorer: Explorer);

        refresh(explorer: Explorer);

        setProperty(prop: any);

        setBitwiseEnumProperty(prop: any, enumValue: number);

        openNewExplorer(obj: IObjectVM);

        results: IObjectVM[];

        selectedItems: IObjectVM[];

    }

    export interface IFresnelService {

        getSession(): ng.IPromise<any>;

        getClassHierarchy(): ng.IPromise<any>;

        createObject(fullyQualifiedName: string): ng.IPromise<any>;

        getObject(request: any): ng.IPromise<any>;

        getProperty(request: any): ng.IPromise<any>;

        setProperty(request: any): ng.IPromise<any>;

        invokeMethod(request: any): ng.IPromise<any>;

        addNewItemToCollection(request: any): ng.IPromise<any>;

        addItemToCollection(request: any): ng.IPromise<any>;

        removeItemFromCollection(request: any): ng.IPromise<any>;

        cleanupSession(): ng.IPromise<any>;
    }

    export interface IObjectVM {

        ID: string;

        IsCollection: boolean;

        Items: IObjectVM[];

        Properties: any[];

        Methods: any[];

        IsMaximised: boolean;

        Type: string;
    }
}