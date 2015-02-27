module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchModalController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'searchService',
            'explorer'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchScope,
            fresnelService: IFresnelService,
            searchService: SearchService,
            explorer: Explorer) {

            $scope.explorer = explorer;
            $scope.results = <SearchResultsVM>explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;

            $scope.searchAction = fresnelService.searchObjects(<SearchObjectsRequest>$scope.request);
            // TODO: Determine which FresnelService.Search() method to use:
            // $scope.searchPromise = fresnelService.SearchPropertyObjects($scope.request);
            // $scope.searchPromise = fresnelService.SearchParameterObjects($scope.request);

            $scope.openNewExplorer = function (obj: ObjectVM) {
                searchService.openNewExplorer(obj, $rootScope);
            }

            $scope.loadNextPage = function () {
                searchService.loadNextPage($scope.request, $scope.results, $scope.searchAction);
            }

            $scope.close = function (explorer: Explorer) {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal: any = $scope;
                modal.$dismiss();
            }

        }

    }
}
