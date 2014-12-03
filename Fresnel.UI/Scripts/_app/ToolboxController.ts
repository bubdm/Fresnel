module FresnelApp {

    export class ToolboxController {
        private http: ng.IHttpService
        private classHierarchy: any;

        constructor(
            $scope: IToolboxControllerScope,
            $http: ng.IHttpService) {

            $scope.message = { title: "Hello World!!" };

            $scope.loadClassHierarchy = function () {
                var uri = "api/Toolbox/GetClassHierarchy";
                $http.get(uri)
                    .success(
                    (data, status) => this.classHierarchy = data);
            }

            $scope.create = function (fullyQualifiedName: string) {
                var uri = "api/Toolbox/Create";
                var arg = "=" + fullyQualifiedName;

                var config = {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }
                };
                $http.post(uri, arg, config)
                    .success(
                    (data, status) => $scope.newInstance = data);
            }

        }
    }

}
