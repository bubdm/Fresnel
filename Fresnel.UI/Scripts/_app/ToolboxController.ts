module FresnelApp {

    export class ToolboxController {
        private http: ng.IHttpService
        private namespaceTree: any;

        constructor(
            $scope: IToolboxControllerScope,
            $http: ng.IHttpService) {

            $scope.message = { title: "Hello World!!" };

            $scope.loadNamespaceTree = function () {
                $http.get("api/Toolbox/GetNamespaceTree")
                    .success(
                    (data, status) => this.namespaceTree = data);
            }

        }
    }
    
}
