module FresnelApp {

    export interface ISearchControllerScope extends ng.IScope {

        explorer: Explorer;

        parentExplorer: Explorer;

        searchResults: SearchResultsVM;

    }

}