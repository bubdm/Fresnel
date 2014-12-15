module FresnelApp {

    export class ToolboxController {
        public classHierarchy: any;

        static $inject = ['$rootScope', '$scope', '$http', 'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            $http: ng.IHttpService,
            appService: AppService) {

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });

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
                    .success(function (data: any, status) {
                        var newObject = data.NewObject;

                        appService.identityMap.addItem(newObject);
                        $rootScope.$broadcast("objectCreated", newObject);
                    });
            }

        }

    }
}