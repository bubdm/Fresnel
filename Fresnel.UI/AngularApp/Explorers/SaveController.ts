/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />

module FresnelApp {

    export class SaveController {

        static $inject = [
            '$rootScope',
            '$scope'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ISaveScope) {

            $scope.yes = function () {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal: any = $scope;
                modal.$close(true);
            };

            $scope.no = function () {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal: any = $scope;
                modal.$close(false);
            };

            $scope.cancel = function () {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal: any = $scope;
                modal.$dismiss();
            };
        }

    }
}
