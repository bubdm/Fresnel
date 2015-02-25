module FresnelApp {

    export class AppController {

        static $inject = ['$scope', 'fresnelService', 'appService', 'inform'];

        constructor(
            $scope: IApplicationScope,
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

            $scope.$on(UiEventType.MessagesReceived, function (event, messages: MessageVM[]) {
                if (messages == null)
                    return;

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

            $scope.$on(UiEventType.ModalOpened, function (event) {
                $scope.IsModalVisible = true;
            });

            $scope.$on(UiEventType.ModalClosed, function (event) {
                $scope.IsModalVisible = false;
            });

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });

        }

    }

}
