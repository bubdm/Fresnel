module FresnelApp {

    export interface ISearchScope extends ICollectionScope {

        explorer: Explorer;

        searchAction: any;

        request: SearchRequest;

        results: SearchResultsVM;

        // This function is called whenever the stTable is changed by the user:
        stTablePipe(tableState: any);

        loadNextPage();

        close(explorer: Explorer);
    }

}