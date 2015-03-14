﻿module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'searchService'];

        private scope: ISearchScope;

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchScope,
            fresnelService: IFresnelService,
            searchService: SearchService) {

            this.scope = $scope;

            $scope.results = <SearchResultsVM>$scope.explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;
            $scope.results.AllowMultiSelect = false;
            $scope.searchAction = function () {
                return fresnelService.searchObjects(<SearchObjectsRequest>$scope.request);
            };

            // This allows Smart-Table to handle the st-safe-src properly:
            $scope.results.DisplayItems = [].concat($scope.results.Items);

            $scope.openNewExplorer = function (obj: ObjectVM) {
                searchService.openNewExplorer(obj, $rootScope);
            }

            $scope.stTablePipe = function (tableState) {
                var predicate: string = tableState.sort.predicate;

                var orderBy: string;
                if (predicate) {
                    var index1 = predicate.indexOf("[");
                    var index2 = predicate.indexOf("]");
                    var propertyIndex = predicate.substr(index1 + 1, index2 - index1 - 1);
                    orderBy = $scope.results.ElementProperties[propertyIndex].InternalName;
                }
                $scope.request.PageNumber = 0;
                $scope.request.OrderBy = orderBy;
                $scope.request.IsDescendingOrder = tableState.sort.reverse;

                $scope.searchAction().then((promiseResult) => {
                    var response = promiseResult.data;

                    // Replace the existing search results:
                    $scope.results.Items = response.Result.Items;

                    // This allows Smart-Table to handle the st-safe-src properly:
                    $scope.results.DisplayItems = [].concat($scope.results.Items);

                    tableState.pagination.numberOfPages = 50;
                });
            };

            $scope.loadNextPage = function () {
                searchService.loadNextPage($scope.request, $scope.results, $scope.searchAction);
            }

        }


    }
}
