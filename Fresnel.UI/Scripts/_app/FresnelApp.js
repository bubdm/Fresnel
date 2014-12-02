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
var FresnelApp;
(function (FresnelApp) {
    var ToolboxController = (function () {
        function ToolboxController($scope, $http) {
            $scope.message = { title: "Hello World!!" };
            $scope.loadClassHierarchy = function () {
                var _this = this;
                $http.get("api/Toolbox/GetClassHierarchy").success(function (data, status) { return _this.classHierarchy = data; });
            };
        }
        return ToolboxController;
    })();
    FresnelApp.ToolboxController = ToolboxController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    angular.module("fresnelApp", []).controller("toolboxController", FresnelApp.ToolboxController).controller("explorerController", FresnelApp.ExplorerController);
})(FresnelApp || (FresnelApp = {}));
