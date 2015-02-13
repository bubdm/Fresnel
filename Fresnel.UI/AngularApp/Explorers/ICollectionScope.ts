module FresnelApp {

    export interface ICollectionScope extends IExplorerScope {

        addNewItem(itemType: string);

        addExistingItems(obj: ObjectVM);

        removeItem(obj: ObjectVM);

    }

}