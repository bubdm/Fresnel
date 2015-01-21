module FresnelApp {

    export class WorkbenchController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService', 'gridsterOptionsFactory'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IWorkbenchControllerScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            gridsterOptionsFactory: any) {

            $scope.visibleExplorers = [];

            $scope.$on('openNewExplorer', function (event, obj: IObjectVM) {
                if (obj) {
                    var existingObj = appService.identityMap.getObject(obj.ID);
                    if (existingObj == null) {
                        appService.identityMap.addObject(obj);
                    }
                    else {
                        // Re-use the existing object, so that any bindings aren't lost:
                        obj = existingObj;
                    }

                    // TODO: Insert the object just after it's parent?
                    var explorer = explorerService.getExplorer(obj.ID);
                    if (explorer == null) {
                        explorer = explorerService.addExplorer(obj);
                        $scope.visibleExplorers.push(explorer);
                    }
                }
            });

            $scope.$on('closeExplorer', function (event, explorer: Explorer) {
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
                    }
                }
            });

            $scope.gridsterOptions = gridsterOptionsFactory.Options;

        }

    }
}
