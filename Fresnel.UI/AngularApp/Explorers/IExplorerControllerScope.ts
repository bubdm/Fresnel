module FresnelApp {

    export interface IExplorerControllerScope extends ng.IScope {

        explorer: Explorer;

        minimise(explorer: Explorer);

        maximise(explorer: Explorer);

        close(explorer: Explorer);

        refresh(explorer: Explorer);

        invoke(method: MethodVM);

        setProperty(prop: PropertyVM);

        associate(prop: PropertyVM);

        disassociate(prop: PropertyVM);

        setBitwiseEnumProperty(prop: PropertyVM, enumValue: number);

        openNewExplorer(obj: ObjectVM);

        openNewExplorerForProperty(prop: PropertyVM);

        save(obj: ObjectVM);
    }

}