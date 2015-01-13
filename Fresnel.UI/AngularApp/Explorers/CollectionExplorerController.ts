module FresnelApp {

    export class CollectionExplorerController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ICollectionExplorerControllerScope,
            fresnelService: IFresnelService,
            appService: AppService) {

            $scope.gridColumns = [];

            var collection: any = $scope.explorer.__meta;

            for (var i = 0; i < collection.ColumnHeaders.length; i++) {
                var newColumn = {
                    name: collection.ColumnHeaders[i].Name,
                    field: 'Properties[' + i + "].Value"
                };
                $scope.gridColumns[i] = newColumn;
            }

            $scope.gridOptions = {
                enableSorting: true,
                columnDefs: $scope.gridColumns,
                data: collection.Items
            };

            $scope.addNewItem = function (itemType: string) {
                var promise = fresnelService.createObject(itemType);

                promise.then((promiseResult) => {
                    var newObject = promiseResult.data.NewObject;

                    appService.identityMap.addObject(newObject);
                    collection.Items.push(newObject);

                    // This will cause the new object to appear in a new Explorer:
                    //$rootScope.$broadcast("objectCreated", newObject);             
                });

            };

        }
    }

}
