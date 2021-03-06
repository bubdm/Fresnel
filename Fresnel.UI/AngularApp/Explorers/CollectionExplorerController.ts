module FresnelApp {

    export class CollectionExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService'
        ];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ICollectionScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
            searchService: SearchService) {

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
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    if (response.Passed) {
                        var latestObj = response.ReturnValue;
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        if (existingObj == null) {
                            appService.identityMap.addObject(latestObj);
                        }
                        else {
                            appService.identityMap.mergeObjects(existingObj, latestObj);
                        }
                        $rootScope.$broadcast(UiEventType.ExplorerOpen, latestObj, $scope.explorer);
                    }
                });
            }

            $scope.addNewItem = function (prop: PropertyVM, itemType: string) {
                var request: CollectionAddNewRequest = {
                    ParentObjectID : prop.ObjectID,
                    CollectionPropertyName: prop.InternalName,
                    ElementTypeName: itemType
                };

                var promise = fresnelService.addNewItemToCollection(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    // This will cause the new object to appear in a new Explorer:
                    $rootScope.$broadcast(UiEventType.ExplorerOpen, response.AddedItem, $scope.explorer);             
                });

            };

            $scope.addExistingItems = function (prop: PropertyVM) {

                var onSelectionConfirmed = function (selectedItems) {
                    var request = requestBuilder.buildAddItemsRequest(prop, selectedItems);
                    var promise = fresnelService.addItemsToCollection(request);
                    promise.then((promiseResult) => {
                        var response = promiseResult.data;

                        appService.identityMap.merge(response.Modifications);
                        $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                    });
                };

                searchService.showSearchForProperty(prop, onSelectionConfirmed);
            };

            $scope.removeItem = function (prop: PropertyVM, obj: ObjectVM) {
                var request: CollectionRemoveRequest = {
                    ParentObjectID: prop.ObjectID,
                    CollectionPropertyName: prop.InternalName,
                    ElementID: obj.ID
                };

                var promise = fresnelService.removeItemFromCollection(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                });

            };
        }

    }
}
