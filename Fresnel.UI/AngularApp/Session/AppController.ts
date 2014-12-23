module FresnelApp {

    export class AppController {

        static $inject = ['$scope', '$http', 'appService'];

        constructor(
            $scope: IAppControllerScope,
            $http: ng.IHttpService,
            appService: AppService) {

            appService.identityMap = new IdentityMap();

            $scope.identityMap = appService.identityMap;

            $scope.loadSession = function () {
                var uri = "api/Session/GetSession";
                $http.get(uri)
                    .success(function (data, status) {
                        $scope.session = data;
                        appService.session = $scope.session;
                    });
            }

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });

        }
    }

}
