module FresnelApp {

    export class AppController {

        static $inject = ['$scope', 'fresnelService', 'appService'];

        constructor(
            $scope: IAppControllerScope,
            fresnelService: IFresnelService,
            appService: AppService) {

            appService.identityMap = new IdentityMap();

            $scope.identityMap = appService.identityMap;

            $scope.loadSession = function () {
                var promise = fresnelService.getSession();

                promise.then((promiseResult) => {
                    var session = promiseResult.data;
                    $scope.session = session;
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
