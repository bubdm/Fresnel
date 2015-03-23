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
            $scope: IExplorerScope,
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
                        $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                        if (response.ResultObject) {
                            $rootScope.$broadcast(UiEventType.ExplorerOpen, response.ResultObject);
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
                                return $scope.explorer;
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

            $scope.setProperty = function (prop: PropertyVM) {
                // BUG: This is to prevent 'digest' model changes accidentally triggering server code:
                // See https://github.com/angular/angular.js/issues/9867
                if ($scope.$$phase == "$digest")
                    return;

                var request = requestBuilder.buildSetPropertyRequest(prop);
                var promise = fresnelService.setProperty(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;
                    prop.Error = response.Passed ? "" : response.Messages[0].Text;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                });
            }

            $scope.setBitwiseEnumProperty = function (prop: PropertyVM, enumValue: number) {
                prop.State.Value = prop.State.Value ^ enumValue;
                $scope.setProperty(prop);
            }

            $scope.isBitwiseEnumPropertySet = function (prop: PropertyVM, enumValue: number) {
                return (prop.State.Value & enumValue) != 0;
            }

            $scope.createAndAssociate = function (prop: PropertyVM, classTypeName: string) {
                var request = requestBuilder.buildCreateAndAssociateRequest(prop, classTypeName);
                var promise = fresnelService.createAndSetProperty(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;
                    prop.Error = response.Passed ? "" : response.Messages[0].Text;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    if (response.Passed) {
                        $rootScope.$broadcast(UiEventType.ExplorerOpen, response.NewObject);
                    }
                });
            }

            $scope.associate = function (prop: PropertyVM) {
                var request = requestBuilder.buildSearchPropertyRequest(prop);
                var promise = fresnelService.searchPropertyObjects(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    var searchResults = response.Result;
                    searchResults.AllowSelection = true;
                    searchResults.AllowMultiSelect = prop.IsCollection;

                    // Set the callback when the user confirms the selection:
                    searchResults.OnSelectionConfirmed = function (items: ObjectVM[]) {
                        if (items.length == 1) {
                            var selectedItem = items[0];
                            prop.State.ReferenceValueID = selectedItem.ID;
                            // Send the request to the server:
                            $scope.setProperty(prop);
                        }
                    }

                    $rootScope.$broadcast(UiEventType.ExplorerOpen, searchResults);
                });
            }

            $scope.disassociate = function (prop: PropertyVM) {
                // If we have a *transient* explorer already open for the property's value, 
                // we need to close it when the property is cleared (not point keeping unwanted objects on screen):
                var transientExplorer: Explorer;
                if (prop.State.Value) {
                    var propertyObjectValue = appService.identityMap.getObject(prop.State.Value.ID);

                    if (propertyObjectValue.IsTransient) {
                        transientExplorer = explorerService.getExplorer(propertyObjectValue.ID);
                    }
                }

                prop.State.Value = null;
                prop.State.ReferenceValueID = null;

                var request = requestBuilder.buildSetPropertyRequest(prop);
                var promise = fresnelService.setProperty(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;
                    prop.Error = response.Passed ? "" : response.Messages[0].Text;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    if (transientExplorer) {
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerClose, transientExplorer);
                    }
                });
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

                $rootScope.$broadcast(UiEventType.ExplorerClose, explorer);
            }

            $scope.openNewExplorer = function (obj: ObjectVM) {
                $rootScope.$broadcast(UiEventType.ExplorerOpen, obj);
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

                        $rootScope.$broadcast(UiEventType.ExplorerOpen, obj);
                    }
                });
            }

            $scope.save = function (obj: ObjectVM) {
                var request = requestBuilder.buildSaveChangesRequest(obj);
                var promise = fresnelService.saveChanges(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                });
            }

        }

    }
}
