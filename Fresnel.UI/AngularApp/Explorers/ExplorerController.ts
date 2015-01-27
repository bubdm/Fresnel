﻿/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />

module FresnelApp {

    export class ExplorerController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService', '$modal'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IExplorerControllerScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            $modal: ng.ui.bootstrap.IModalService) {

            $scope.invoke = function (method: any) {
                if (method.Parameters.length == 0) {
                    var promise = fresnelService.invokeMethod(method);

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
                        resolve: {
                            // These objects will be available on the MethodController's scope:
                            explorer: function () {
                                return $scope.explorer;
                            },
                            method: function () {
                                return method;
                            }
                        }
                    }
                    var modal = $modal.open(options);

                    modal.result.then(() => {
                        fresnelService.invokeMethod(method);
                    });
                }
            }

            $scope.setProperty = function (prop: any) {
                var request = {
                    ObjectId: prop.ObjectID,
                    PropertyName: prop.PropertyName,
                    NonReferenceValue: prop.State.Value
                };
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
                var request = {
                    ObjectId: explorer.__meta.ID,
                };
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

            $scope.openNewExplorer = function (obj: IObjectVM) {
                $rootScope.$broadcast("openNewExplorer", obj);
            }

            $scope.openNewExplorerForProperty = function (prop: any) {
                var promise = fresnelService.getProperty(prop);

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
