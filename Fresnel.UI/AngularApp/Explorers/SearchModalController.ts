module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchModalController {

        static $inject = [
            '$rootScope',
            '$scope',
            'searchService',
            'explorer'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchScope,
            searchService: SearchService,
            explorer: Explorer) {

            $scope.explorer = explorer;
            $scope.results = <SearchResultsVM>explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;

            $scope.openNewExplorer = function (obj: ObjectVM) {
                searchService.openNewExplorer(obj, $rootScope);
            }

            $scope.loadNextPage = function () {
                searchService.loadNextPage($scope.request, $scope.results);
            }

        }

    }
}
