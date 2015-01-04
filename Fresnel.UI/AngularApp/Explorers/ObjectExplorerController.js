var FresnelApp;
(function (FresnelApp) {
    var ObjectExplorerController = (function () {
        function ObjectExplorerController($scope, $http, appService) {
            $scope.visibleExplorers = [];
            $scope.$on('objectCreated', function (event, obj) {
                $scope.visibleExplorers.push(obj);
            });
            $scope.invoke = function (method) {
                var uri = "api/Explorer/InvokeMethod";
                $http.post(uri, method).success(function (data, status) {
                    appService.identityMap.merge(data.Modifications);
                });
            };
            $scope.setProperty = function (prop) {
                var uri = "api/Explorer/SetProperty";
                var request = {
                    ObjectId: prop.ObjectID,
                    PropertyName: prop.PropertyName,
                    NonReferenceValue: prop.Value
                };
                $http.post(uri, request).success(function (data, status) {
                    appService.identityMap.merge(data.Modifications);
                });
            };
            $scope.minimise = function (obj) {
                obj.IsMaximised = false;
            };
            $scope.maximise = function (obj) {
                obj.IsMaximised = true;
            };
            $scope.close = function (obj) {
                // TODO: Check for dirty status
                var index = $scope.visibleExplorers.indexOf(obj);
                if (index > -1) {
                    $scope.visibleExplorers.splice(index, 1);
                }
            };
            $scope.openNewExplorer = function (prop) {
                var uri = "api/Explorer/GetObjectProperty";
                $http.post(uri, prop).success(function (data, status) {
                    var obj = data.ReturnValue;
                    if (obj) {
                        appService.identityMap.addItem(obj);
                        // TODO: Insert the object just after it's parent?
                        obj.OuterProperty = prop;
                        $scope.visibleExplorers.push(obj);
                    }
                });
            };
        }
        ObjectExplorerController.$inject = ['$scope', '$http', 'appService'];
        return ObjectExplorerController;
    })();
    FresnelApp.ObjectExplorerController = ObjectExplorerController;
})(FresnelApp || (FresnelApp = {}));
//# sourceMappingURL=ObjectExplorerController.js.map