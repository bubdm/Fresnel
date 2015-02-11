
module FresnelApp {

    export class SearchService {

        static $inject = [
            '$rootScope',
            'fresnelService',
            'explorerService',
            'requestBuilder',
            '$modal'];

        constructor(
            $rootScope: ng.IRootScopeService,
            fresnelService: IFresnelService,
            explorerService: ExplorerService,
            requestBuilder: RequestBuilder,
            $modal: ng.ui.bootstrap.IModalService) {

            this.showSearchForCollection = function (coll: CollectionVM, onSelectionConfirmed) {
                var request = requestBuilder.buildSearchObjectsRequest(coll.ElementType);
                var searchPromise = fresnelService.searchObjects(request);

                searchPromise.then((promiseResult) => {
                    var response = promiseResult.data;
                    var searchResults: CollectionVM = response.Result;
                    var searchExplorer = explorerService.addExplorer(searchResults);

                    var options: ng.ui.bootstrap.IModalSettings = {
                        templateUrl: '/Templates/searchResultsExplorer.html',
                        controller: 'searchExplorerController',
                        backdrop: 'static',
                        resolve: {
                            // These objects will be injected into the SearchController's ctor:
                            explorer: function () {
                                return searchExplorer;
                            }
                        }
                    }

                    var modal = $modal.open(options);
                    $rootScope.$broadcast("modalOpened", modal);

                    modal.result.then(() => {
                        var selectedItems = $.grep(searchResults.Items, function (o: SearchResultItemVM) {
                            return o.IsSelected;
                        });
                        if (selectedItems.length > 0) {
                            onSelectionConfirmed(selectedItems);
                        }
                    });

                    modal.result.finally(() => {
                        $rootScope.$broadcast("modalClosed", modal);
                    });
                });

            }
        }

        showSearchForCollection = function (coll: CollectionVM, onSelectionConfirmed) { }

        showSearchForProperty = function (prop: PropertyVM, onSelectionConfirmed) { }

        showSearchForParameter = function (param: ParameterVM, onSelectionConfirmed) { }

    }
}
