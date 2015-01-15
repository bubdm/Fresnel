module FresnelApp {

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

        getObject(request: any): ng.IPromise<any> {
            var uri = "api/Explorer/GetObject";
            return this.http.post(uri, request);
        }

        getProperty(request: any): ng.IPromise<any> {
            var uri = "api/Explorer/GetObjectProperty";
            return this.http.post(uri, request);
        }

        setProperty(request: any): ng.IPromise<any> {
            var uri = "api/Explorer/SetProperty";
            return this.http.post(uri, request);
        }

        invokeMethod(request: any): ng.IPromise<any> {
            var uri = "api/Explorer/InvokeMethod";
            return this.http.post(uri, request);
        }

        cleanupSession(): ng.IPromise<any> {
            var uri = "api/Session/CleanUp";
            return this.http.get(uri);
        }

    }

}