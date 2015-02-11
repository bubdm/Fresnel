module FresnelApp {

    export class CollectionExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ICollectionExplorerControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService) {

            var collection = <CollectionVM>$scope.explorer.__meta;
            // This allows Smart-Table to handle the st-safe-src properly:
            collection.DisplayItems = [].concat(collection.Items);

            // NB: This overrides the function in ExplorerController
            $scope.openNewExplorer = function (obj: ObjectVM) {
                // As the collection only contains a lightweight object, we need to fetch one with more detail:
                var request = requestBuilder.buildGetObjectRequest(obj);
                var promise = fresnelService.getObject(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast("messagesReceived", response.Messages);

                    if (response.Passed) {
                        var latestObj = response.ReturnValue;
                        appService.identityMap.addObject(latestObj);
                        $rootScope.$broadcast("openNewExplorer", latestObj);
                    }
                });
            }

            $scope.addNewItem = function (itemType: string) {
                var request: CollectionAddNewRequest = {
                    CollectionID: collection.ID,
                    ElementTypeName: itemType,
                };

                var promise = fresnelService.addNewItemToCollection(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast("messagesReceived", response.Messages);

                    // This will cause the new object to appear in a new Explorer:
                    //$rootScope.$broadcast("openNewExplorer", newObject);             
                });

            };

            $scope.addExistingItems = function (coll: CollectionVM) {
                var request = requestBuilder.buildSearchObjectsRequest(coll.ElementType);
                var promiseSearch = fresnelService.searchObjects(request);

                promiseSearch.then((promiseResult) => {
                    var response = promiseResult.data;

                    $rootScope.$broadcast("messagesReceived", response.Messages);

                    if (response.Passed) {
                        var searchResult = response.Result;

                        // Set the callback when the user confirms the selection:
                        searchResult.OnSelectionConfirmed = function (items: ObjectVM[]) {
                            var addItemsRequest = requestBuilder.buildAddItemsRequest(coll, items);
                            var promiseAdd = fresnelService.addItemsToCollection(addItemsRequest);
                            promiseAdd.then((promiseResult) => {
                                var response = promiseResult.data;

                                appService.identityMap.merge(response.Modifications);
                                $rootScope.$broadcast("messagesReceived", response.Messages);
                            });
                        }

                        $rootScope.$broadcast("openNewExplorer", searchResult);
                    }
                });
            };

            $scope.removeItem = function (obj: ObjectVM) {
                var request: CollectionRemoveRequest = {
                    CollectionID: collection.ID,
                    ElementID: obj.ID
                };

                var promise = fresnelService.removeItemFromCollection(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast("messagesReceived", response.Messages);
                });

            };
        }

        getEditorTemplate(prop) {
            if (prop.Info == null)
                return;

            switch (prop.Info.Name) {
                case "boolean":
                    switch (prop.Info.PreferredControl) {
                        default:
                            return '/Templates/Editors/booleanRadioEditor.html';
                    }

                case "datetime":
                    switch (prop.Info.PreferredControl) {
                        case "Date":
                            return '/Templates/Editors/dateEditor.html';
                        case "Time":
                            return '/Templates/Editors/timeEditor.html';
                        default:
                            return '/Templates/Editors/dateTimeEditor.html';
                    }

                case "enum":
                    switch (prop.Info.PreferredControl) {
                        case "Checkbox":
                            return '/Templates/Editors/enumCheckboxEditor.html';
                        case "Radio":
                            return '/Templates/Editors/enumRadioEditor.html';
                        default:
                            return '/Templates/Editors/enumSelectEditor.html';
                    }

                case "string":
                    switch (prop.Info.PreferredControl) {
                        case "Password":
                            return '/Templates/Editors/passwordEditor.html';
                        case "TextArea":
                            return '/Templates/Editors/textAreaEditor.html';
                        case "RichTextArea":
                            return '/Templates/Editors/richTextEditor.html';
                        default:
                            return '/Templates/Editors/stringEditor.html';
                    }

                default:
                    return '';
            }
        }

    }
}
