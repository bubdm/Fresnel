module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchModalController {

        static $inject = [
            '$rootScope',
            '$scope',
            'searchService',
            'fresnelService',
            'requestBuilder',
            'appService',
            'explorer'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchScope,
            searchService: SearchService,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
            explorer: Explorer) {

            $scope.explorer = explorer;
            $scope.results = <SearchResultsVM>explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;

            $scope.openNewExplorer = function (obj: ObjectVM) {

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
