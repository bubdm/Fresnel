
module FresnelApp {

    export class SearchService {

        static $inject = [
            '$rootScope',
            'fresnelService',
            'appService',
            'explorerService',
            'requestBuilder',
            '$modal'];

        constructor(
            $rootScope: ng.IRootScopeService,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            requestBuilder: RequestBuilder,
            $modal: ng.ui.bootstrap.IModalService) {

            this.showSearchForCollection = function (coll: CollectionVM, onSelectionConfirmed) {
                var request = requestBuilder.buildSearchObjectsRequest(coll.ElementType);
                var searchPromise = fresnelService.searchObjects(request);

                // TODO: Open the modal _before_ the search is executed:

                searchPromise.then((promiseResult) => {
                    var response = promiseResult.data;
                    var searchResults: SearchResultsVM = response.Result;
                    searchResults.OriginalRequest = request;
                    var searchExplorer = explorerService.addExplorer(searchResults);

                    var options: ng.ui.bootstrap.IModalSettings = {
                        templateUrl: '/Templates/searchResultsExplorer.html',
                        controller: 'searchModalController',
                        backdrop: 'static',
                        size: 'lg',
                        resolve: {
                            // These objects will be injected into the SearchController's ctor:
                            explorer: function () {
                                return searchExplorer;
                            }
                        }
                    }

                    var modal = $modal.open(options);
                    $rootScope.$broadcast(UiEventType.ModalOpened, modal);

                    modal.result.then(() => {
                        var selectedItems = $.grep(searchResults.Items, function (o: SearchResultItemVM) {
                            return o.IsSelected;
                        });
                        if (selectedItems.length > 0) {
                            onSelectionConfirmed(selectedItems);
                        }
                    });

                    modal.result.finally(() => {
                        $rootScope.$broadcast(UiEventType.ModalClosed, modal);
                    });
                });

                this.openNewExplorer = function (obj: ObjectVM, $rootScope: ng.IScope) {

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
                            appService.identityMap.mergeObjects(existingObj, latestObj);
                            $rootScope.$broadcast(UiEventType.ExplorerOpen, latestObj);
                        }
                    });
                }

                this.loadNextPage = function ($scope: ISearchScope) {
                    $scope.request.PageNumber++;

                    var promise = fresnelService.searchObjects($scope.request);

                    promise.then((promiseResult) => {
                        var response = promiseResult.data;
                        var newSearchResults: SearchResultsVM = response.Result;

                        // Append the new items to the exist results:
                        var existingSearchResults = $scope.results;

                        for (var i = 0; i < newSearchResults.Items.length; i++) {
                            existingSearchResults.Items.push(newSearchResults.Items[i]);
                        }
                    });
                }

            }
        }

        showSearchForCollection = function (coll: CollectionVM, onSelectionConfirmed) { }

        showSearchForProperty = function (prop: PropertyVM, onSelectionConfirmed) { }

        showSearchForParameter = function (param: ParameterVM, onSelectionConfirmed) { }

        openNewExplorer = function (obj: ObjectVM, $rootScope: ng.IScope) { }

        loadNextPage = function ($scope: ISearchScope) { }

    }
}
