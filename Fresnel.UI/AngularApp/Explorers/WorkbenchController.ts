module FresnelApp {

    export class WorkbenchController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IWorkbenchScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService) {

            $scope.visibleRows = [];

            $scope.$on(UiEventType.ExplorerOpen, function (event, obj: ObjectVM, parentExplorer: Explorer) {
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

                var newExplorer = explorerService.getExplorer(obj.ID);
                if (newExplorer == null) {
                    newExplorer = explorerService.addExplorer(obj);

                    // Determine which row to put the new Explorer in:
                    var explorerRow: ExplorerRow = (parentExplorer == undefined || parentExplorer == null) ?
                        null :
                        parentExplorer.ParentRow;

                    if (explorerRow == undefined || explorerRow == null) {
                        explorerRow = {
                            Explorers: [],
                            ColourIndex: $scope.visibleRows.length % 8
                        };
                        $scope.visibleRows.push(explorerRow);
                    }

                    // Determine the position of the new Explorer in it's row:
                    var panelIndex = explorerRow.Explorers.indexOf(parentExplorer);
                    var isPanelInserted = false;
                    if (panelIndex > -1) {
                        var newIndex = panelIndex + 1;
                        if (newIndex != explorerRow.Explorers.length) {
                            explorerRow.Explorers.splice(panelIndex + 1, 0, newExplorer);
                            isPanelInserted = true;
                        }
                    }

                    if (!isPanelInserted) {
                        explorerRow.Explorers.push(newExplorer);
                    }

                    newExplorer.ParentRow = explorerRow;
                    newExplorer.ParentExplorer = parentExplorer;
                }
            });

            $scope.$on(UiEventType.ExplorerClose, function (event, explorer: Explorer) {
                var parentRow = explorer.ParentRow;

                var panelIndex = parentRow.Explorers.indexOf(explorer);
                if (panelIndex > -1) {
                    parentRow.Explorers.splice(panelIndex, 1);

                    // Dispose of the Explorer:
                    explorerService.remove(explorer);
                    explorer.ParentRow = null;
                    explorer.ParentExplorer = null;

                    // Determine if the row needs to disappear:
                    if (parentRow.Explorers.length == 0) {
                        var rowIndex = $scope.visibleRows.indexOf(parentRow);
                        $scope.visibleRows.splice(rowIndex, 1);
                    }

                    // Clean up the Session if necessary:
                    if ($scope.visibleRows.length == 0) {
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
