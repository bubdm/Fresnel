module FresnelApp {

    export interface ICollectionScope extends IExplorerScope {

        addNewItem(prop: PropertyVM, itemType: string);

        addExistingItems(prop: PropertyVM, obj: ObjectVM);

        removeItem(prop: PropertyVM, obj: ObjectVM);

    }

}