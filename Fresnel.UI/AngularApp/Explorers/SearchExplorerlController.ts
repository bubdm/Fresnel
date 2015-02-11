module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'explorer',
            'parentExplorer',
            'searchResults'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            explorer: Explorer,
            parentExplorer: Explorer,
            searchResults: SearchResultsVM) {

            $scope.explorer = explorer;
            $scope.parentExplorer = parentExplorer;
            $scope.searchResults = searchResults;
            
        }

    }
}
