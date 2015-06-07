module FresnelApp {

    export class FresnelService implements IFresnelService {

        private http: ng.IHttpService;
        private blockUI: any;

        static $inject = ['$http', 'blockUI'];

        constructor($http: ng.IHttpService,
            blockUi: any) {
            this.http = $http;
            this.blockUI = blockUi;
        }

        getSession(): ng.IPromise<any> {
            this.blockUI.start("Starting your session...");

            var uri = "api/Session/GetSession";
            var promise = this.http.get(uri);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        getClassHierarchy(): ng.IPromise<any> {
            this.blockUI.start("Setting up Library...");

            var uri = "api/Toolbox/GetClassHierarchy";
            var promise = this.http.get(uri);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        getDomainServicesHierarchy(): ng.IPromise<any> {
            this.blockUI.start("Setting up Services...");

            var uri = "api/Toolbox/GetDomainServicesHierarchy";
            var promise = this.http.get(uri);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        createObject(request: CreateObjectRequest): ng.IPromise<any> {
            this.blockUI.start("Creating new object...");

            var uri = "api/Toolbox/Create";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        getObject(request: GetObjectRequest): ng.IPromise<any> {
            this.blockUI.start("Retrieving object...");

            var uri = "api/Explorer/GetObject";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        getProperty(request: GetPropertyRequest): ng.IPromise<any> {
            this.blockUI.start("Loading property...");

            var uri = "api/Explorer/GetObjectProperty";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        createAndSetProperty(request: CreateAndSetPropertyRequest): ng.IPromise<any> {
            this.blockUI.start("Creating new object...");

            var uri = "api/Explorer/CreateAndSetProperty";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        setProperty(request: SetPropertyRequest): ng.IPromise<any> {
            this.blockUI.start("Setting property value...");

            var uri = "api/Explorer/SetProperty";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        setParameter(request: SetParameterRequest): ng.IPromise<any> {
            this.blockUI.start("Setting parameter value...");

            var uri = "api/Explorer/SetParameter";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        invokeMethod(request: InvokeMethodRequest): ng.IPromise<any> {
            this.blockUI.start("Performing action...");

            var uri = "api/Explorer/InvokeMethod";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        addNewItemToCollection(request: CollectionAddNewRequest): ng.IPromise<any> {
            this.blockUI.start("Adding new item to collection...");

            var uri = "api/Explorer/AddNewItemToCollection";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        addItemsToCollection(request: CollectionAddRequest): ng.IPromise<any> {
            this.blockUI.start("Adding items to collection...");

            var uri = "api/Explorer/AddItemsToCollection";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        removeItemFromCollection(request: CollectionRemoveRequest): ng.IPromise<any> {
            this.blockUI.start("Removing item from collection...");

            var uri = "api/Explorer/RemoveItemFromCollection";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        cleanupSession(): ng.IPromise<any> {
            this.blockUI.start("Cleaning up your workbench...");

            var uri = "api/Session/CleanUp";
            var promise = this.http.get(uri);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        saveChanges(request: SaveChangesRequest): ng.IPromise<any> {
            this.blockUI.start("Saving all changes...");

            var uri = "api/Explorer/SaveChanges";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        cancelChanges(request: CancelChangesRequest): ng.IPromise<any> {
            this.blockUI.start("Cancelling changes...");

            var uri = "api/Explorer/CancelChanges";
            var promise = this.http.post(uri, request);

            promise.finally(() => {
                this.blockUI.stop();
            });

            return promise;
        }

        searchObjects(request: SearchObjectsRequest): ng.IPromise<any> {
            var uri = "api/Toolbox/SearchObjects";
            var promise = this.http.post(uri, request);
            return promise;
        }

        searchPropertyObjects(request: SearchPropertyRequest): ng.IPromise<any> {
            var uri = "api/Explorer/SearchPropertyObjects";
            var promise = this.http.post(uri, request);
            return promise;
        }

        searchParameterObjects(request: SearchParameterRequest): ng.IPromise<any> {
            var uri = "api/Explorer/SearchParameterObjects";
            var promise = this.http.post(uri, request);
            return promise;
        }

    }

}