﻿module FresnelApp {

    export class FresnelService implements IFresnelService {

        private http: ng.IHttpService;

        static $inject = ['$http'];

        constructor($http: ng.IHttpService) {
            this.http = $http;
        }

        getSession(): ng.IPromise<any> {
            var uri = "api/Session/GetSession";
            return this.http.get(uri);
        }

        getClassHierarchy(): ng.IPromise<any> {
            var uri = "api/Toolbox/GetClassHierarchy";
            return this.http.get(uri);
        }

        createObject(fullyQualifiedName: string): ng.IPromise<any> {
            var uri = "api/Toolbox/Create";
            var arg = "=" + fullyQualifiedName;

            var config = {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }
            };
            return this.http.post(uri, arg, config);
        }

        getObject(request: GetObjectRequest): ng.IPromise<any> {
            var uri = "api/Explorer/GetObject";
            return this.http.post(uri, request);
        }

        getObjects(request: GetObjectsRequest): ng.IPromise<any> {
            var uri = "api/Toolbox/GetObjects";
            return this.http.post(uri, request);
        }

        getProperty(request: GetPropertyRequest): ng.IPromise<any> {
            var uri = "api/Explorer/GetObjectProperty";
            return this.http.post(uri, request);
        }

        setProperty(request: SetPropertyRequest): ng.IPromise<any> {
            var uri = "api/Explorer/SetProperty";
            return this.http.post(uri, request);
        }

        invokeMethod(request: InvokeMethodRequest): ng.IPromise<any> {
            var uri = "api/Explorer/InvokeMethod";
            return this.http.post(uri, request);
        }

        addNewItemToCollection(request: CollectionAddNewRequest): ng.IPromise<any> {
            var uri = "api/Explorer/AddNewItemToCollection";
            return this.http.post(uri, request);
        }

        addItemsToCollection(request: CollectionAddRequest): ng.IPromise<any> {
            var uri = "api/Explorer/AddItemsToCollection";
            return this.http.post(uri, request);
        }

        removeItemFromCollection(request: CollectionRemoveRequest): ng.IPromise<any> {
            var uri = "api/Explorer/RemoveItemFromCollection";
            return this.http.post(uri, request);
        }

        cleanupSession(): ng.IPromise<any> {
            var uri = "api/Session/CleanUp";
            return this.http.get(uri);
        }

        saveChanges(request: SaveChangesRequest): ng.IPromise<any> {
            var uri = "api/Explorer/SaveChanges";
            return this.http.post(uri, request);
        }
        
        searchObjects(request: SearchObjectsRequest): ng.IPromise<any> {
            var uri = "api/Toolbox/SearchObjects";
            return this.http.post(uri, request);
        }

    }

}