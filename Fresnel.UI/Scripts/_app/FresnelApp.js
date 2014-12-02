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
            $scope.loadNamespaceTree = function () {
                var _this = this;
                $http.get("api/Toolbox/GetNamespaceTree").success(function (data, status) { return _this.namespaceTree = data; });
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
