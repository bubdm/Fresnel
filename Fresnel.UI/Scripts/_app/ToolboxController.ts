module FresnelApp {

    export class ToolboxController {
        private http: ng.IHttpService
        private namespaceHierarchy: any;

        constructor(
            $scope: IToolboxControllerScope,
            $http: ng.IHttpService) {

            this.http = $http;

            $scope.message = { title: "Hello World!!" };

            this.getNamespaceHierarchy();
        }

        getNamespaceHierarchy() {
            var uri = "/GetNamespaceHierarchy";

            this.http.get(uri)
                .success(
                (data, status) => this.namespaceHierarchy = data);
        }

    }
}
