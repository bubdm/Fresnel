module FresnelApp {

    export interface IFresnelService {

        getSession(): ng.IPromise<any>;

        getClassHierarchy(): ng.IPromise<any>;

        createObject(request: CreateObjectRequest): ng.IPromise<any>;

        getObject(request: GetObjectRequest): ng.IPromise<any>;

        getProperty(request: GetPropertyRequest): ng.IPromise<any>;

        createAndSetProperty(request: CreateAndSetPropertyRequest): ng.IPromise<any>;

        setProperty(request: SetPropertyRequest): ng.IPromise<any>;

        setParameter(request: SetParameterRequest): ng.IPromise<any>;

        invokeMethod(request: InvokeMethodRequest): ng.IPromise<any>;

        invokeDependencyMethod(request: InvokeDependencyMethodRequest): ng.IPromise<any>;

        addNewItemToCollection(request: CollectionAddNewRequest): ng.IPromise<any>;

        addItemsToCollection(request: CollectionAddRequest): ng.IPromise<any>;

        removeItemFromCollection(request: CollectionRemoveRequest): ng.IPromise<any>;

        cleanupSession(): ng.IPromise<any>;

        saveChanges(request: SaveChangesRequest): ng.IPromise<any>;

        cancelChanges(request: CancelChangesRequest): ng.IPromise<any>;

        searchObjects(request: SearchObjectsRequest): ng.IPromise<any>;

        searchPropertyObjects(request: SearchPropertyRequest): ng.IPromise<any>;

        searchParameterObjects(request: SearchParameterRequest): ng.IPromise<any>;
    }

}