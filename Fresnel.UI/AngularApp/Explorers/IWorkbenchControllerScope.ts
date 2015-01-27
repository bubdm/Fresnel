module FresnelApp {

    export interface IExplorerControllerScope extends ng.IScope {

        explorer: Explorer;

        minimise(explorer: Explorer);

        maximise(explorer: Explorer);

        close(explorer: Explorer);

        refresh(explorer: Explorer);

        invoke(method: MethodVM);

        setProperty(prop: ValueVM);

        setBitwiseEnumProperty(prop: ValueVM, enumValue: number);

        openNewExplorer(obj: ObjectVM);

        openNewExplorerForProperty(prop: ValueVM);

    }

}