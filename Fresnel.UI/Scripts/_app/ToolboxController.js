/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="interfaces.ts" />
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
//# sourceMappingURL=ToolboxController.js.map