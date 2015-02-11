
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

            this.showSearchForCollection = function (parentExplorer: Explorer, fullTypeName: string) {
                var request = requestBuilder.buildSearchObjectsRequest(fullTypeName);
                var promiseSearch = fresnelService.searchObjects(request);

                promiseSearch.then((promiseResult) => {
                    var response = promiseResult.data;
                    var searchResults = response.Result;
                    var searchExplorer = explorerService.addExplorer(searchResults);

                    var options: ng.ui.bootstrap.IModalSettings = {
                        templateUrl: '/Templates/searchResultsExplorer.html',
                        controller: 'searchExplorerController',
                        backdrop: 'static',
                        resolve: {
                            // These objects will be injected into the SearchController's ctor:
                            explorer: function () {
                                return searchExplorer;
                            },
                            parentExplorer: function () {
                                return parentExplorer;
                            },
                            searchResults: function () {
                                return searchResults;
                            }
                        }
                    }

                    var modal = $modal.open(options);
                    $rootScope.$broadcast("modalOpened", modal);

                    modal.result.then(() => {
                        //var addItemsRequest = requestBuilder.buildAddItemsRequest(coll, items);
                        //var promiseAdd = fresnelService.addItemsToCollection(addItemsRequest);
                        //promiseAdd.then((promiseResult) => {
                        //    var response = promiseResult.data;

                        //    appService.identityMap.merge(response.Modifications);
                        //    $rootScope.$broadcast("messagesReceived", response.Messages);
                        //});
                    });

                    modal.result.finally(() => {
                        $rootScope.$broadcast("modalClosed", modal);
                    });
                });

            }
        }

        showSearchForCollection = function (parentExplorer: Explorer, elementTypeName: string) { }

        showSearchForProperty = function (parentExplorer: Explorer, prop: PropertyVM) { }

        showSearchForParameter = function (parentExplorer: Explorer, param: ParameterVM) { }

    }
}
