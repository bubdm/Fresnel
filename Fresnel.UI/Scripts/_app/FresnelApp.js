var FresnelApp;
(function (FresnelApp) {
    var AppController = (function () {
        function AppController($scope, appService) {
            appService.identityMap = new FresnelApp.IdentityMap();
            $scope.identityMap = appService.identityMap;
        }
        return AppController;
    })();
    FresnelApp.AppController = AppController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var AppService = (function () {
        function AppService() {
        }
        return AppService;
    })();
    FresnelApp.AppService = AppService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var ExplorerController = (function () {
        function ExplorerController($scope, $http, appService) {
            $scope.visibleExplorers = [];
            $scope.$on('objectCreated', function (event, obj) {
                $scope.visibleExplorers.push(obj);
            });
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
                        $scope.visibleExplorers.push(obj);
                    }
                });
            };
        }
        ExplorerController.$inject = ['$scope', '$http', 'appService'];
        return ExplorerController;
    })();
    FresnelApp.ExplorerController = ExplorerController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var IdentityMap = (function () {
        function IdentityMap() {
            this.hash = [];
            this.items = [];
        }
        IdentityMap.prototype.getItem = function (key) {
            var item = this.hash[key];
            return item;
        };
        IdentityMap.prototype.addItem = function (obj) {
            this.removeItem(obj.ID);
            this.items.push({
                key: obj.ID,
                value: obj
            });
            this.hash[obj.ID] = obj;
        };
        IdentityMap.prototype.removeItem = function (key) {
            var index = this.items.indexOf(key);
            if (index > -1) {
                this.items.splice(index, 1);
                delete this.hash[key];
            }
        };
        IdentityMap.prototype.merge = function (delta) {
        };
        return IdentityMap;
    })();
    FresnelApp.IdentityMap = IdentityMap;
    var IdentityMapDelta = (function () {
        function IdentityMapDelta() {
        }
        return IdentityMapDelta;
    })();
    FresnelApp.IdentityMapDelta = IdentityMapDelta;
})(FresnelApp || (FresnelApp = {}));
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
        }
        ToolboxController.$inject = ['$rootScope', '$scope', '$http', 'appService'];
        return ToolboxController;
    })();
    FresnelApp.ToolboxController = ToolboxController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    angular.module("fresnelApp", ['ngDock']).service("appService", FresnelApp.AppService).controller("appController", FresnelApp.AppController).controller("toolboxController", FresnelApp.ToolboxController).controller("explorerController", FresnelApp.ExplorerController);
})(FresnelApp || (FresnelApp = {}));
