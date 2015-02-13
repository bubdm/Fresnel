module FresnelApp {

    export interface ISearchControllerScope extends ng.IScope {

        explorer: Explorer;

        request: SearchObjectsRequest;

        results: SearchResultsVM;

        loadNextPage();

        close(explorer: Explorer);
    }

}