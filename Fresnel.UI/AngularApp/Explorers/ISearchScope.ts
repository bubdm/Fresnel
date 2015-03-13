module FresnelApp {

    export interface ISearchScope extends ICollectionScope {

        explorer: Explorer;

        searchAction: any;

        request: SearchRequest;

        results: SearchResultsVM;

        loadNextPage();

        close(explorer: Explorer);
    }

}