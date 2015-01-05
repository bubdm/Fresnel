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
                    .success(function (data : Session, status) {
                        $scope.session = data;
                    });
            }

            $scope.$on('messagesReceived', function (event, messageSet: any) {
                $scope.session.TestValue++;
                appService.mergeMessages(messageSet, $scope.session);
            });

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });

        }

    }

}
