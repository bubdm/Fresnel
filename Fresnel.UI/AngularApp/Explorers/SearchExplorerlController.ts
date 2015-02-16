module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService) {

            $scope.results = <SearchResultsVM>$scope.explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;

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
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        appService.identityMap.mergeObjects(existingObj, latestObj);
                        $rootScope.$broadcast("openNewExplorer", latestObj);
                    }
                });
            }

            $scope.loadNextPage = function () {
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
}
