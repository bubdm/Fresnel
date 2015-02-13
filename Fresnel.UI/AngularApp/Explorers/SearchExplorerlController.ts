module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'explorer',
            'request',
            'results'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            explorer: Explorer,
            request: SearchObjectsRequest,
            results: SearchResultsVM) {

            $scope.explorer = explorer;
            $scope.request = request;
            $scope.results = results;

            $scope.loadNextPage = function () {
                $scope.request.PageNumber++;

                var promise = fresnelService.searchObjects(request);

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

            $scope.close = function (explorer: Explorer) {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal: any = $scope;
                modal.$dismiss();
            }

        }

    }
}
