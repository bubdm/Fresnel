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
            $scope: ICollectionExplorerControllerScope,
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

                var onSelectionConfirmed = function (selectedItems) {
                    var request = requestBuilder.buildAddItemsRequest(coll, selectedItems);
                    var promise = fresnelService.addItemsToCollection(request);
                    promise.then((promiseResult) => {
                        var response = promiseResult.data;

                        appService.identityMap.merge(response.Modifications);
                        $rootScope.$broadcast("messagesReceived", response.Messages);
                    });
                };

                searchService.showSearchForCollection(coll, onSelectionConfirmed);
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

    }
}
