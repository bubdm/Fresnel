module FresnelApp {

    export interface ICollectionExplorerControllerScope extends IExplorerControllerScope {

        addNewItem(itemType: string);

        addExistingItems(obj: ObjectVM);

        removeItem(obj: ObjectVM);

    }

}