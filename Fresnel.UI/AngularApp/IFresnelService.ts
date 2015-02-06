module FresnelApp {

    export interface IFresnelService {

        getSession(): ng.IPromise<any>;

        getClassHierarchy(): ng.IPromise<any>;

        createObject(fullyQualifiedName: string): ng.IPromise<any>;

        getObject(request: GetObjectRequest): ng.IPromise<any>;

        getObjects(request: GetObjectsRequest): ng.IPromise<any>;

        getProperty(request: GetPropertyRequest): ng.IPromise<any>;

        setProperty(request: SetPropertyRequest): ng.IPromise<any>;

        invokeMethod(request: InvokeMethodRequest): ng.IPromise<any>;

        addNewItemToCollection(request: CollectionRequest): ng.IPromise<any>;

        addItemToCollection(request: CollectionRequest): ng.IPromise<any>;

        removeItemFromCollection(request: CollectionRequest): ng.IPromise<any>;

        cleanupSession(): ng.IPromise<any>;

        saveChanges(request: SaveChangesRequest): ng.IPromise<any>;
    }

}