module FresnelApp {

    export interface ISearchControllerScope extends ng.IScope {

        explorer: Explorer;

        close(explorer: Explorer);
    }

}