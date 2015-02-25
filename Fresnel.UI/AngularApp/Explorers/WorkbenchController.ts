module FresnelApp {

    export class WorkbenchController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IWorkbenchScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService) {

            $scope.visibleExplorers = [];

            $scope.$on(UiEventType.ExplorerOpen, function (event, obj: ObjectVM) {
                if (!obj)
                    return;

                // Re-use the existing object, so that any bindings aren't lost:
                var existingObj = appService.identityMap.getObject(obj.ID);
                if (existingObj != null) {
                    obj = existingObj;
                }
                else {
                    appService.identityMap.addObject(obj);
                }

                var explorer = explorerService.getExplorer(obj.ID);
                if (explorer == null) {
                    explorer = explorerService.addExplorer(obj);
                    $scope.visibleExplorers.push(explorer);
                }
            });

            $scope.$on(UiEventType.ExplorerClose, function (event, explorer: Explorer) {
                var index = $scope.visibleExplorers.indexOf(explorer);
                if (index > -1) {
                    $scope.visibleExplorers.splice(index, 1);

                    explorerService.remove(explorer);
                    if ($scope.visibleExplorers.length == 0) {
                        var promise = fresnelService.cleanupSession();

                        promise.then((promiseResult) => {
                            var response = promiseResult.data;
                            appService.identityMap.reset(); 
                            $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                        });
                    }
                }
            });

        }

    }
}
