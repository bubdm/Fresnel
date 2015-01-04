var FresnelApp;
(function (FresnelApp) {
    var ToolboxController = (function () {
        function ToolboxController($rootScope, $scope, $http, appService) {
            $scope.loadClassHierarchy = function () {
                var _this = this;
                var uri = "api/Toolbox/GetClassHierarchy";
                $http.get(uri).success(function (data, status) { return _this.classHierarchy = data; });
            };
            $scope.create = function (fullyQualifiedName) {
                var uri = "api/Toolbox/Create";
                var arg = "=" + fullyQualifiedName;
                var config = {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }
                };
                $http.post(uri, arg, config).success(function (data, status) {
                    var newObject = data.NewObject;
                    appService.identityMap.addItem(newObject);
                    $rootScope.$broadcast("objectCreated", newObject);
                });
            };
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });
        }
        ToolboxController.$inject = ['$rootScope', '$scope', '$http', 'appService'];
        return ToolboxController;
    })();
    FresnelApp.ToolboxController = ToolboxController;
})(FresnelApp || (FresnelApp = {}));
//# sourceMappingURL=ToolboxController.js.map