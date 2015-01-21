module FresnelApp {

    export class ObjectExplorerController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService', 'gridsterOptionsFactory'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IObjectExplorerControllerScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            gridsterOptionsFactory: any) {

            $scope.visibleExplorers = [];

            $scope.$on('showObject', function (event, obj: IObjectVM) {
                var explorer = explorerService.getExplorer(obj.ID);
                if (explorer == null) {
                    explorer = explorerService.addExplorer(obj);
                    $scope.visibleExplorers.push(explorer);
                }
            });

            $scope.invoke = function (method: any) {
                var promise = fresnelService.invokeMethod(method);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;
                    method.Error = result.Passed ? "" : result.Messages[0].Text;

                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);

                    if (result.ResultObject) {
                        $rootScope.$broadcast("showObject", result.ResultObject);
                    }
                });
            }

            $scope.setProperty = function (prop: any) {
                var request = {
                    ObjectId: prop.ObjectID,
                    PropertyName: prop.PropertyName,
                    NonReferenceValue: prop.State.Value
                };
                var promise = fresnelService.setProperty(request);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;
                    prop.Error = result.Passed ? "" : result.Messages[0].Text;

                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                });
            }

            $scope.setBitwiseEnumProperty = function (prop: any, enumValue: number) {
                prop.State.Value = prop.State.Value ^ enumValue;
                $scope.setProperty(prop);
            }

            $scope.refresh = function (explorer: Explorer) {
                var request = {
                    ObjectId: explorer.__meta.ID,
                };
                var promise = fresnelService.getObject(request);

                promise.then((promiseResult) => {
                    var obj = promiseResult.data.ReturnValue;
                    var existingObj = appService.identityMap.getObject(obj.ID);

                    appService.identityMap.mergeObjects(existingObj, obj);
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

                    explorerService.remove(explorer);
                    if ($scope.visibleExplorers.length == 0) {
                        var promise = fresnelService.cleanupSession();

                        promise.then((promiseResult) => {
                            var result = promiseResult.data;
                            $rootScope.$broadcast("messagesReceived", result.Messages);
                        });

                        fresnelService.cleanupSession();
                    }
                }
            }

            $scope.openNewExplorer = function (prop: any) {
                var promise = fresnelService.getProperty(prop);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;

                    var obj = result.ReturnValue;
                    if (obj) {
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        if (existingObj == null) {
                            appService.identityMap.addObject(obj);
                        }

                        obj.OuterProperty = prop;

                        // TODO: Insert the object just after it's parent?
                        var explorer = explorerService.getExplorer(obj.ID);
                        if (explorer == null) {
                            explorer = explorerService.addExplorer(obj);
                            $scope.visibleExplorers.push(explorer);
                        }
                    }
                });

            }

            $scope.gridsterOptions = gridsterOptionsFactory.Options;

        }

    }
}
