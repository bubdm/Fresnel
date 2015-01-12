module FresnelApp {

    export class ObjectExplorerController {

        static $inject = ['$rootScope', '$scope', '$http', '$timeout', 'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IObjectExplorerControllerScope,
            $http: ng.IHttpService,
            $timeout: ng.ITimeoutService,
            appService: AppService) {

            $scope.visibleExplorers = [];

            $scope.$on('objectCreated', function (event, obj: IObjectVM) {
                var explorer = appService.identityMap.getExplorer(obj.ID);
                $scope.visibleExplorers.push(explorer);
            });

            $scope.invoke = function (method: any) {
                var uri = "api/Explorer/InvokeMethod";

                $http.post(uri, method)
                    .success(function (data: any, status) {
                        appService.identityMap.merge(data.Modifications);
                        $rootScope.$broadcast("messagesReceived", data.Messages);
                    });
            }

            $scope.setProperty = function (prop: any) {
                var uri = "api/Explorer/SetProperty";

                var request = {
                    ObjectId: prop.ObjectID,
                    PropertyName: prop.PropertyName,
                    NonReferenceValue: prop.Value
                };

                $http.post(uri, request)
                    .success(function (data: any, status) {
                        $timeout(function () {
                            appService.identityMap.merge(data.Modifications);
                            $rootScope.$broadcast("messagesReceived", data.Messages);
                        })
                    });
            }

            $scope.minimise = function (explorer: Explorer) {
                explorer.IsMaximised = false;
            }

            $scope.maximise = function (explorer: Explorer) {
                explorer.IsMaximised = true;
            }

            $scope.close = function (explorer: Explorer) {
                // TODO: Check for dirty status

                var index = $scope.visibleExplorers.indexOf(explorer);
                if (index > -1) {
                    $scope.visibleExplorers.splice(index, 1);

                    // TODO: If the object is no longer in the UI, Let the server know that it can be GCed
                }
            }

            $scope.openNewExplorer = function (prop: any) {
                var uri = "api/Explorer/GetObjectProperty";

                $http.post(uri, prop)
                    .success(function (data: any, status) {
                        var obj = data.ReturnValue;
                        if (obj) {
                            appService.identityMap.addObject(obj);

                            // TODO: Insert the object just after it's parent?
                            obj.OuterProperty = prop;

                            var explorer = appService.identityMap.getExplorer(obj.ID);
                            $scope.visibleExplorers.push(explorer);
                        }
                    });
            }

        }
    }
}
