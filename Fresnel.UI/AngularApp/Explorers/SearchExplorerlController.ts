module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder) {

            $scope.results = <SearchResultsVM>$scope.explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;

            $scope.loadNextPage = function () {
                $scope.request.PageNumber++;

                var promise = fresnelService.searchObjects($scope.request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;
                    var newSearchResults: CollectionVM = response.Result;

                    // Append the new items to the exist results:
                    // TODO: T4TS doesn't convert sub-classes correctly, 
                    //       so it doesn't know that $scope.results is derived from CollectionVM
                    var existingSearchResults: any = $scope.results;

                    for (var i = 0; i < newSearchResults.Items.length; i++) {
                        existingSearchResults.Items.push(newSearchResults.Items[i]);
                    }
                });
            }

        }

    }
}
