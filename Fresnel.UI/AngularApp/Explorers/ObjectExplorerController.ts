module FresnelApp {

    export class ObjectExplorerController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IObjectExplorerControllerScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService) {

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

            $scope.gridsterOptions = {
                columns: 12, // the width of the grid, in columns
                pushing: true, // whether to push other items out of the way on move or resize
                floating: true, // whether to automatically float items up so they stack (you can temporarily disable if you are adding unsorted items with ng-repeat)
                swapping: false, // whether or not to have items of the same size switch places instead of pushing down if they are the same size
                width: 'auto', // can be an integer or 'auto'. 'auto' scales gridster to be the full width of its containing element
                colWidth: 'auto', // can be an integer or 'auto'.  'auto' uses the pixel width of the element divided by 'columns'
                rowHeight: 'match', // can be an integer or 'match'.  Match uses the colWidth, giving you square widgets.
                margins: [10, 10], // the pixel distance between each widget
                outerMargin: true, // whether margins apply to outer edges of the grid
                isMobile: false, // stacks the grid items if true
                mobileBreakPoint: 600, // if the screen is not wider that this, remove the grid layout and stack the items
                mobileModeEnabled: true, // whether or not to toggle mobile mode when screen width is less than mobileBreakPoint
                minColumns: 1, // the minimum columns the grid must have
                minRows: 2, // the minimum height of the grid, in rows
                maxRows: 100,
                defaultSizeX: 4, // the default width of a gridster item, if not specifed
                defaultSizeY: 1, // the default height of a gridster item, if not specified
                minSizeX: 4, // minimum column width of an item
                maxSizeX: null, // maximum column width of an item
                minSizeY: 1, // minumum row height of an item
                maxSizeY: null, // maximum row height of an item
                resizable: {
                    enabled: true,
                    handles: ['s', 'se', 'e'] // ['n', 'e', 's', 'w', 'ne', 'se', 'sw', 'nw']
                },
                draggable: {
                    enabled: true, // whether dragging items is supported
                    handle: '.dragHandle', // optional selector for resize handle
                }
            };

        }

    }
}
