module FresnelApp {

    export interface ICollectionExplorerControllerScope extends IExplorerControllerScope {

        addNewItem(itemType: string);

        addExistingItem(obj: ObjectVM);

        removeItem(obj: ObjectVM);

    }

}