module FresnelApp {

    export class ToolboxController {
        private http: ng.IHttpService
        private classHierarchy: any;

        constructor(
            $scope: IToolboxControllerScope,
            $http: ng.IHttpService) {

            $scope.message = { title: "Hello World!!" };

            $scope.loadClassHierarchy = function () {
                $http.get("api/Toolbox/GetClassHierarchy")
                    .success(
                    (data, status) => this.classHierarchy = data);
            }

        }
    }
    
}
