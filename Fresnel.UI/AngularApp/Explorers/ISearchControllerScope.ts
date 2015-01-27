module FresnelApp {

    export interface ISearchControllerScope extends ng.IScope {

        explorer: Explorer;

        searchType: string;

        close(explorer: Explorer);

        refresh(explorer: Explorer);

        setProperty(prop: ValueVM);

        setBitwiseEnumProperty(prop: ValueVM, enumValue: number);

        openNewExplorer(obj: ObjectVM);

        results: ObjectVM[];

        selectedItems: ObjectVM[];

    }

}