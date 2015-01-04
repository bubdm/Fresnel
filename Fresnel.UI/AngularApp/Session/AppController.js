var FresnelApp;
(function (FresnelApp) {
    var AppController = (function () {
        function AppController($scope, $http, appService) {
            appService.identityMap = new FresnelApp.IdentityMap();
            $scope.identityMap = appService.identityMap;
            $scope.loadSession = function () {
                var uri = "api/Session/GetSession";
                $http.get(uri).success(function (data, status) {
                    $scope.session = data;
                    appService.session = $scope.session;
                });
            };
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });
        }
        AppController.$inject = ['$scope', '$http', 'appService'];
        return AppController;
    })();
    FresnelApp.AppController = AppController;
})(FresnelApp || (FresnelApp = {}));
//# sourceMappingURL=AppController.js.map