﻿/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />

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

            $scope.invoke = function (method: MethodVM) {
                if (method.Parameters.length == 0) {
                    var request = requestBuilder.buildMethodInvokeRequest(method);
                    var promise = fresnelService.invokeMethod(request);

                    promise.then((promiseResult) => {
                        var response = promiseResult.data;
                        method.Error = response.Passed ? "" : response.Messages[0].Text;

                        appService.identityMap.merge(response.Modifications);
                        $rootScope.$broadcast("messagesReceived", response.Messages);

                        if (response.ResultObject) {
                            $rootScope.$broadcast("openNewExplorer", response.ResultObject);
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

            $scope.setProperty = function (prop: PropertyVM) {
                var request = requestBuilder.buildSetPropertyRequest(prop);
                var promise = fresnelService.setProperty(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;
                    prop.Error = response.Passed ? "" : response.Messages[0].Text;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast("messagesReceived", response.Messages);
                });
            }

            $scope.setBitwiseEnumProperty = function (prop: PropertyVM, enumValue: number) {
                prop.State.Value = prop.State.Value ^ enumValue;
                $scope.setProperty(prop);
            }

            $scope.isBitwiseEnumPropertySet = function (prop: PropertyVM, enumValue: number) {
                return (prop.State.Value & enumValue) != 0;
            }

            $scope.associate = function (prop: PropertyVM) {
                var request = requestBuilder.buildSearchObjectsRequest(prop.Info.FullTypeName);
                var promise = fresnelService.searchObjects(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    var searchResults = response.Result;

                    // Set the callback when the user confirms the selection:
                    searchResults.OnSelectionConfirmed = function (items: ObjectVM[]) {
                        if (items.length == 1) {
                            var selectedItem = items[0];
                            prop.State.ReferenceValueID = selectedItem.ID;
                            // Send the request to the server:
                            $scope.setProperty(prop);
                        }
                    }

                    $rootScope.$broadcast("openNewExplorer", searchResults);
                });
            }

            $scope.disassociate = function (prop: PropertyVM) {
                prop.State.Value = null;
                prop.State.ReferenceValueID = null;
                $scope.setProperty(prop);
            }

            $scope.refresh = function (explorer: Explorer) {
                var request = requestBuilder.buildGetObjectRequest(explorer.__meta);
                var promise = fresnelService.getObject(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;
                    var obj = response.ReturnValue;
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

            $scope.openNewExplorerForProperty = function (prop: PropertyVM) {
                var request = requestBuilder.buildGetPropertyRequest(prop);
                var promise = fresnelService.getProperty(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    var obj = response.ReturnValue;
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

            $scope.save = function (obj: ObjectVM) {
                var request = requestBuilder.buildSaveChangesRequest(obj);
                var promise = fresnelService.saveChanges(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast("messagesReceived", response.Messages);
                });
            }

        }

    }
}
