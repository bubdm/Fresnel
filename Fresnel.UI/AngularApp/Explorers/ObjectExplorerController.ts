module FresnelApp {

    export class ObjectExplorerController {

        static $inject = ['$scope', '$http', 'appService'];

        constructor(
            $scope: IObjectExplorerControllerScope,
            $http: ng.IHttpService,
            appService: AppService) {

            $scope.visibleExplorers = [];

            $scope.$on('objectCreated', function (event, obj: IObjectVM) {
                attachMembers(obj);
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
                            attachMembers(obj);
                            obj.OuterProperty = prop;
                            $scope.visibleExplorers.push(obj);
                        }
                    });
            }

            function attachMembers(obj: IObjectVM) {
                if (obj.IsCollection) {
                    for (var i = 0; i < obj.Items.length; i++) {
                        attachMembers(obj.Items[i]);
                    }
                }

                if (obj.Properties) {
                    for (var i = 0; i < obj.Properties.length; i++) {
                        var prop = obj.Properties[i];
                        obj[prop.PropertyName] = prop;
                    }
                }

                if (obj.Methods) {
                    for (var i = 0; i < obj.Methods.length; i++) {
                        var method = obj.Methods[i];
                        obj[method.MethodName] = method;
                    }
                }
            }

        }
    }
}
