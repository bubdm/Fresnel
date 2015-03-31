/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />

module FresnelApp {

    export class ExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService',
            'explorerService',
            'saveService',
            '$modal'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IExplorerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
            searchService: SearchService,
            explorerService: ExplorerService,
            saveService: SaveService,
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
                            $rootScope.$broadcast(UiEventType.ExplorerOpen, response.ResultObject, $scope.explorer);
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

                    var obj = appService.identityMap.getObject(prop.ObjectID);
                    obj.DirtyState.IsDirty = true;
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

                    var obj = appService.identityMap.getObject(prop.ObjectID);
                    obj.DirtyState.IsDirty = true;

                    if (response.Passed) {
                        $rootScope.$broadcast(UiEventType.ExplorerOpen, response.NewObject, $scope.explorer);
                    }
                });
            }

            $scope.associate = function (prop: PropertyVM) {
                var onSelectionConfirmed = function (selectedItems) {
                    if (selectedItems.length == 1) {
                        var selectedItem = selectedItems[0];
                        prop.State.ReferenceValueID = selectedItem.ID;
                        
                        // NB: We can't call the setProperty() function, as a digest is already running.
                        //     Hence the need to in-line the code here:
                        var request = requestBuilder.buildSetPropertyRequest(prop);
                        var promise = fresnelService.setProperty(request);

                        promise.then((promiseResult) => {
                            var response = promiseResult.data;
                            prop.Error = response.Passed ? "" : response.Messages[0].Text;

                            appService.identityMap.merge(response.Modifications);
                            $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                            var obj = appService.identityMap.getObject(prop.ObjectID);
                            obj.DirtyState.IsDirty = true;
                        });
                    }
                };

                searchService.showSearchForProperty(prop, onSelectionConfirmed);
            }

            $scope.disassociate = function (prop: PropertyVM) {
                // If we have a *transient* explorer already open for the property's value, 
                // we need to close it when the property is cleared (not point keeping unwanted objects on screen):
                var transientExplorer: Explorer;
                if (prop.State.Value) {
                    var propertyObjectValue = appService.identityMap.getObject(prop.State.Value.ID);

                    if (propertyObjectValue.DirtyState.IsTransient) {
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

                    var obj = appService.identityMap.getObject(prop.ObjectID);
                    obj.DirtyState.IsDirty = true;

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
                var obj = explorer.__meta;

                if (!saveService.isRequiredFor(obj)) {
                    // Just close the window:
                    $rootScope.$broadcast(UiEventType.ExplorerClose, explorer);
                    return;
                }

                // See what the user wants to do:
                var promise = saveService.askUser(obj);
                promise.then((isSaveRequested) => {
                    if (isSaveRequested) {
                        promise = saveService.invoke(obj);
                    }
                    promise.finally(() => {
                        $rootScope.$broadcast(UiEventType.ExplorerClose, explorer);
                    });
                });
            }

            $scope.openNewExplorer = function (obj: ObjectVM) {
                $rootScope.$broadcast(UiEventType.ExplorerOpen, obj, $scope.explorer);
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

                        // TODO: Using obj.OuterProperty has a limitation. Two objects may refer to the same object.
                        //       So the OuterProperty needs to be attached to the Explorer, NOT the Object itself:
                        obj.OuterProperty = prop;

                        $rootScope.$broadcast(UiEventType.ExplorerOpen, obj, $scope.explorer);
                    }
                });
            }

            $scope.save = function (obj: ObjectVM) {
                saveService.invoke(obj);
            }

        }

    }
}
