module FresnelApp {

    export interface ISearchScope extends ng.IScope {

        explorer: Explorer;

        request: SearchObjectsRequest;

        results: SearchResultsVM;

        loadNextPage();

        close(explorer: Explorer);
    }

}