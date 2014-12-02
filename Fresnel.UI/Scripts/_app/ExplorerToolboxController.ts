/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="interfaces.ts" />

module FresnelApp {
    export class ExplorerController {
        constructor($scope: IExplorerControllerScope) {
            $scope.message = { title: "Goodbye!!" };
        }
    }
}
