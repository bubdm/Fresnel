module FresnelApp {

    export interface IExplorerScope extends ng.IScope {

        explorer: Explorer;

        minimise(explorer: Explorer);

        maximise(explorer: Explorer);

        close(explorer: Explorer);

        refresh(explorer: Explorer);

        invoke(method: MethodVM);

        setProperty(prop: PropertyVM);

        createAndAssociate(prop: PropertyVM, classTypeName: string);

        associate(prop: PropertyVM);

        disassociate(prop: PropertyVM);

        setBitwiseEnumProperty(prop: PropertyVM, enumValue: number);

        isBitwiseEnumPropertySet(prop: PropertyVM, enumValue: number) : boolean;

        openNewExplorer(obj: ObjectVM);

        openNewExplorerForProperty(prop: PropertyVM);

        save(obj: ObjectVM);
    }

}