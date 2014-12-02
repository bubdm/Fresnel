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
            this.http = $http;
            $scope.message = { title: "Hello World!!" };
            this.getNamespaceHierarchy();
        }
        ToolboxController.prototype.getNamespaceHierarchy = function () {
            var _this = this;
            var uri = "/GetNamespaceHierarchy";
            this.http.get(uri).success(function (data, status) { return _this.namespaceHierarchy = data; });
        };
        return ToolboxController;
    })();
    FresnelApp.ToolboxController = ToolboxController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    angular.module("fresnelApp", []).controller("ToolboxController", FresnelApp.ToolboxController).controller("ExplorerController", FresnelApp.ExplorerController);
})(FresnelApp || (FresnelApp = {}));
//# sourceMappingURL=FresnelApp.js.map