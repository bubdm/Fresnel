module FresnelApp {

    export class ToolboxController {
        public classHierarchy: Namespace[];

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService',
            'blockUI',
            '$modal'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
            searchService: SearchService,
            blockUI: any,
            $modal: ng.ui.bootstrap.IModalService) {

            $scope.loadClassHierarchy = function () {
                var promise = fresnelService.getClassHierarchy();

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    this.classHierarchy = promiseResult.data;
                });
            }

            $scope.create = function (fullyQualifiedName: string) {
                var request = requestBuilder.buildCreateObjectRequest(null, fullyQualifiedName);
                var promise = fresnelService.createObject(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    if (response.Passed) {
                        var newObject = response.NewObject;
                        appService.identityMap.addObject(newObject);
                        $rootScope.$broadcast(UiEventType.ExplorerOpen, newObject);
                    }
                });
            }

            $scope.searchObjects = function (fullyQualifiedName: string) {
                searchService.searchForObjects(fullyQualifiedName);
            }

            $scope.invokeDependencyMethod = function (method: DependencyMethodVM) {
                if (method.Parameters.length == 0) {
                    var request = requestBuilder.buildInvokeDependencyMethodRequest(method);
                    var promise = fresnelService.invokeDependencyMethod(request);

                    promise.then((promiseResult) => {
                        var response = promiseResult.data;
                        method.Error = response.Passed ? "" : response.Messages[0].Text;

                        appService.identityMap.merge(response.Modifications);
                        $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                        if (response.ResultObject) {
                            $rootScope.$broadcast(UiEventType.ExplorerOpen, response.ResultObject, null);
                        }
                    });
                }
                else {
                    var options: ng.ui.bootstrap.IModalSettings = {
                        templateUrl: '/Templates/methodDialog.html',
                        controller: 'methodController',
                        backdrop: 'static',
                        size: 'lg',
                        resolve: {
                            // These objects will be injected into the MethodController's ctor:
                            explorer: function () {
                                return null;
                            },
                            method: function () {
                                return method;
                            }
                        }
                    }

                    var modal = $modal.open(options);
                    $rootScope.$broadcast(UiEventType.ModalOpened, modal);

                    modal.result.finally(() => {
                        $rootScope.$broadcast(UiEventType.ModalClosed, modal);
                    });
                }
            }

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });

        }

    }
}