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

        openNewExplorer(prop: any);
    }


    export interface IObjectVM {

        ID: string;

        IsMaximised: boolean;
    }
}