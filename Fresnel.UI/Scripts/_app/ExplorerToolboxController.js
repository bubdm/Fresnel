/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="interfaces.ts" />
var FresnelApp;
(function (FresnelApp) {
    var ExplorerController = (function () {
        function ExplorerController($scope) {
            $scope.message = { title: "Goodbye!!" };
        }
        return ExplorerController;
    })();
    FresnelApp.ExplorerController = ExplorerController;
})(FresnelApp || (FresnelApp = {}));
//# sourceMappingURL=ExplorerToolboxController.js.map