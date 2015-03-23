/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />

module FresnelApp {

    export class MethodController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'appService',
            'explorerService',
            'searchService',
            'requestBuilder',
            'explorer',
            'method'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IMethodScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            searchService: SearchService,
            requestBuilder: RequestBuilder,
            explorer: Explorer,
            method: MethodVM) {

            $scope.explorer = explorer;
            $scope.method = method;

            method.ParametersSetByUser = [];

            $scope.invoke = function (method: MethodVM) {
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

            $scope.setProperty = function (param: ParameterVM) {
                // Find the parameter, and set it's value:
                // If it's an Object, set the ReferenceValueID
                if (!param.IsNonReference) {
                    param.State.ReferenceValueID = param.State.Value.ID;
                }

                var matches = $.grep(method.ParametersSetByUser, function (e) {
                    return e == param;
                });
                if (matches.length == 0) {
                    method.ParametersSetByUser.push(param);
                }
            }

            $scope.setBitwiseEnumProperty = function (param: ParameterVM, enumValue: number) {
                param.State.Value = param.State.Value ^ enumValue;
                $scope.setProperty(param);
            }

            $scope.isBitwiseEnumPropertySet = function (param: ParameterVM, enumValue: number) {
                return (param.State.Value & enumValue) != 0;
            }

            $scope.associate = function (param: ParameterVM) {
                var onSelectionConfirmed = function (selectedItems) {
                    if (selectedItems.length == 1) {
                        var selectedItem = selectedItems[0];
                        param.State.ReferenceValueID = selectedItem.ID;
                        
                        // NB: We can't call the setProperty() function, as a digest is already running.
                        //     Hence the need to in-line the code here:
                        var obj = $scope.explorer.__meta;
                        var method = $scope.method;
                        var request = requestBuilder.buildSetParameterRequest(obj, method, param);
                        var promise = fresnelService.setParameter(request);

                        promise.then((promiseResult) => {
                            var response = promiseResult.data;
                            param.Error = response.Passed ? "" : response.Messages[0].Text;

                            $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                        });
                    }
                };

                searchService.showSearchForParameter(method, param, onSelectionConfirmed);
            }

            $scope.addExistingItems = function (param: ParameterVM, coll: CollectionVM) {

                var onSelectionConfirmed = function (selectedItems) {
                    // TODO

                    //var request = requestBuilder.buildAddItemsRequest(coll, selectedItems);
                    //var promise = fresnelService.addItemsToCollection(request);
                    //promise.then((promiseResult) => {
                    //    var response = promiseResult.data;

                    //    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                    //});
                };

                searchService.showSearchForParameter(method, param, onSelectionConfirmed);
            };

            $scope.close = function (explorer: Explorer) {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal: any = $scope;
                modal.$dismiss();
            }

        }

    }
}
