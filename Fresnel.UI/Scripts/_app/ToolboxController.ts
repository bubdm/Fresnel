module FresnelApp {

    export class ToolboxController {
        public classHierarchy: any;

        static $inject = ['$rootScope', '$scope', '$http', 'appService']; 

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            $http: ng.IHttpService,
            appService: AppService) {

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
                    .success(function (data: IObjectVM, status) {
                        appService.identityMap.add(data);
                        $rootScope.$broadcast("objectCreated", data);
                    });
            }
        }

    }
}