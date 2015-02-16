module FresnelApp {

    export interface ISearchScope extends ICollectionScope {

        explorer: Explorer;

        request: SearchObjectsRequest;

        results: SearchResultsVM;

        loadNextPage();

        close(explorer: Explorer);
    }

}