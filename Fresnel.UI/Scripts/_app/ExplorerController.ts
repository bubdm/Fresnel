module FresnelApp {

    export class ExplorerController {

        static $inject = ['$scope', '$http', 'appService'];

        constructor(
            $scope: IExplorerControllerScope,
            $http: ng.IHttpService,
            appService: AppService) {

            $scope.visibleExplorers = [];

            $scope.$on('objectCreated', function (event, obj: IObjectVM) {
                $scope.visibleExplorers.push(obj);
            });

            $scope.invoke = function (method: any) {
                var uri = "api/Explorer/InvokeMethod";

                $http.post(uri, method)
                    .success(function (data: any, status) {
                        //var obj = data.ReturnValue;
                        //if (obj) {
                        //    appService.identityMap.addItem(obj);

                        //    // TODO: Insert the object just after it's parent?
                        //    $scope.visibleExplorers.push(obj);
                        //}
                    });
            }

            $scope.minimise = function (obj: IObjectVM) {
                obj.IsMaximised = false;
            }

            $scope.maximise = function (obj: IObjectVM) {
                obj.IsMaximised = true;
            }

            $scope.close = function (obj: IObjectVM) {
                // TODO: Check for dirty status

                var index = $scope.visibleExplorers.indexOf(obj);
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
                            appService.identityMap.addItem(obj);

                            // TODO: Insert the object just after it's parent?
                            $scope.visibleExplorers.push(obj);
                        }
                    });
            }

        }
    }

}
