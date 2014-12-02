/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
var FresnelApp;
(function (FresnelApp) {
    var HomeController = (function () {
        function HomeController($scope) {
            $scope.message = { title: "Hello World!!" };
        }
        return HomeController;
    })();
    FresnelApp.HomeController = HomeController;
    var ExplorerController = (function () {
        function ExplorerController($scope) {
            $scope.message = { title: "Goodbye!!" };
        }
        return ExplorerController;
    })();
    FresnelApp.ExplorerController = ExplorerController;
})(FresnelApp || (FresnelApp = {}));
angular.module("fresnelApp", []).controller("HomeController", FresnelApp.HomeController).controller("ExplorerController", FresnelApp.ExplorerController);
//# sourceMappingURL=FresnelApp.js.map