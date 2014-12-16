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

        visibleExplorers: any[];

        minimise(obj: IObjectVM);

        maximise(obj: IObjectVM);

        close(obj: IObjectVM);

        invoke(method: IObjectVM);

        openNewExplorer(prop: any);

        setupCollectionGrid(collectionVM, gridElementName);
    }


    export interface IObjectVM {

        ID: string;

        IsMaximised: boolean;
    }
}