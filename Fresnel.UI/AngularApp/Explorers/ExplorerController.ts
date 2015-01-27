/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />

module FresnelApp {

    export class ExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'explorerService',
            '$modal'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IExplorerControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
            explorerService: ExplorerService,
            $modal: ng.ui.bootstrap.IModalService) {

            $scope.invoke = function (method: any) {
                if (method.Parameters.length == 0) {
                    var request = requestBuilder.buildMethodInvokeRequest(method);
                    var promise = fresnelService.invokeMethod(request);

                    promise.then((promiseResult) => {
                        var result = promiseResult.data;
                        method.Error = result.Passed ? "" : result.Messages[0].Text;

                        appService.identityMap.merge(result.Modifications);
                        $rootScope.$broadcast("messagesReceived", result.Messages);

                        if (result.ResultObject) {
                            $rootScope.$broadcast("openNewExplorer", result.ResultObject);
                        }
                    });
                }
                else {
                    var options: ng.ui.bootstrap.IModalSettings = {
                        templateUrl: '/Templates/methodDialog.html',
                        controller: 'methodController',
                        backdrop: 'static',
                        resolve: {
                            // These objects will be injected into the MethodController's ctor:
                            explorer: function () {
                                return $scope.explorer;
                            },
                            method: function () {
                                return method;
                            }
                        }
                    }

                    var modal = $modal.open(options);
                    $rootScope.$broadcast("modalOpened", modal);

                    modal.result.finally(() => {
                        $rootScope.$broadcast("modalClosed", modal);
                    });
                }
            }

            $scope.setProperty = function (prop: any) {
                var request = requestBuilder.buildSetPropertyRequest(prop);
                var promise = fresnelService.setProperty(request);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;
                    prop.Error = result.Passed ? "" : result.Messages[0].Text;

                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                });
            }

            $scope.setBitwiseEnumProperty = function (prop: any, enumValue: number) {
                prop.State.Value = prop.State.Value ^ enumValue;
                $scope.setProperty(prop);
            }

            $scope.refresh = function (explorer: Explorer) {
                var request = requestBuilder.buildGetObjectRequest(explorer.__meta);
                var promise = fresnelService.getObject(request);

                promise.then((promiseResult) => {
                    var obj = promiseResult.data.ReturnValue;
                    var existingObj = appService.identityMap.getObject(obj.ID);

                    appService.identityMap.mergeObjects(existingObj, obj);
                });
            }

            $scope.minimise = function (explorer: Explorer) {
                explorer.IsMaximised = false;
            }

            $scope.maximise = function (explorer: Explorer) {
                explorer.IsMaximised = true;
            }

            $scope.close = function (explorer: Explorer) {
                // TODO: Check for dirty status

                $rootScope.$broadcast("closeExplorer", explorer);
            }

            $scope.openNewExplorer = function (obj: ObjectVM) {
                $rootScope.$broadcast("openNewExplorer", obj);
            }

            $scope.openNewExplorerForProperty = function (prop: any) {
                var request = requestBuilder.buildGetPropertyRequest(prop);
                var promise = fresnelService.getProperty(request);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;

                    var obj = result.ReturnValue;
                    if (obj) {
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        if (existingObj == null) {
                            appService.identityMap.addObject(obj);
                        }
                        else {
                            // Re-use the existing object, so that any bindings aren't lost:
                            obj = existingObj;
                        }

                        obj.OuterProperty = prop;

                        $rootScope.$broadcast("openNewExplorer", obj);
                    }
                });
            }

        }

    }
}
