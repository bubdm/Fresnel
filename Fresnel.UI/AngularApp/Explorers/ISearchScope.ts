module FresnelApp {

    export interface ISearchScope extends ICollectionScope {

        explorer: Explorer;

        searchAction: ng.IPromise<any>;

        request: SearchRequest;

        results: SearchResultsVM;

        loadNextPage();

        close(explorer: Explorer);
    }

}