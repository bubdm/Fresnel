/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="interfaces.ts" />

module FresnelApp {

    export class ToolboxController {
        constructor($scope: IToolboxControllerScope) {
            $scope.message = { title: "Hello World!!" };

        }
    }

}
