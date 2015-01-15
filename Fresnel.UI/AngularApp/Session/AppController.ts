module FresnelApp {

    export class AppController {

        static $inject = ['$scope', 'fresnelService', 'appService', 'inform'];

        constructor(
            $scope: IAppControllerScope,
            fresnelService: IFresnelService,
            appService: AppService,
            inform: any) {

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


                for (var i = 0; i < messageSet.Infos.length; i++) {
                    var message = messageSet.Infos[i];
                    inform.add(message.Text, { type: 'success' });
                }

                for (var i = 0; i < messageSet.Warnings.length; i++) {
                    var message = messageSet.Warnings[i];
                    inform.add(message.Text, { type: 'warning' });
                }

                for (var i = 0; i < messageSet.Errors.length; i++) {
                    var message = messageSet.Errors[i];
                    inform.add(message.Text, { type: 'danger' });
                }
            });

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });

        }

    }

}
