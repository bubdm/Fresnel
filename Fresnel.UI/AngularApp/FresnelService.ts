module FresnelApp {

    export class FresnelService implements IFresnelService {

        private http: ng.IHttpService;

        static $inject = ['$http'];

        constructor($http: ng.IHttpService) {
            this.http = $http;
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

    }

}