module FresnelApp {

    // Used to control the interactions within an open Search form
    export class SearchExplorerController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'explorer'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISearchControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            explorer: Explorer) {

            $scope.explorer = explorer;

            $scope.close = function (explorer: Explorer) {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal: any = $scope;
                modal.$dismiss();
            }

        }

    }
}
