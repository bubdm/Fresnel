module FresnelApp {

    export class CollectionExplorerController {

        static $inject = ['$rootScope', '$scope', '$http', 'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ICollectionExplorerControllerScope,
            $http: ng.IHttpService,
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
                // TODO: This is copied from ToolboxController, and needs refactoring:
                var uri = "api/Toolbox/Create";
                var arg = "=" + itemType;

                var config = {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }
                };
                $http.post(uri, arg, config)
                    .success(function (data: any, status) {
                        var newObject = data.NewObject;

                        appService.identityMap.addObject(newObject);
                        collection.Items.push(newObject);

                        // This will cause the new object to appear in a new Explorer:
                        //$rootScope.$broadcast("objectCreated", newObject);
                    });
            };

        }
    }

}
