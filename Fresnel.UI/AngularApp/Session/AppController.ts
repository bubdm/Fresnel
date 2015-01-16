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

            $scope.$on('messagesReceived', function (event, messages: any) {
                $scope.session.TestValue++;
                appService.mergeMessages(messages, $scope.session);

                for (var i = 0; i < messages.length; i++) {
                    var message = messages[i];
                    var messageType =
                        message.IsSuccess ? 'success' :
                        message.IsInfo ? 'info' :
                        message.IsWarning ? 'warning' :
                        message.IsError ? 'danger' :
                        'default';

                    // Don't let the message automatically fade away:
                    var messageTtl = message.RequiresAcknowledgement ? 0 : 5000;

                    inform.add(message.Text, { ttl: messageTtl, type: messageType });
                }
            });

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });

        }

    }

}
