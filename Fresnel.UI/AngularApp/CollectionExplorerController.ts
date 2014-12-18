module FresnelApp {

    export class CollectionExplorerController {

        static $inject = ['$scope', '$http', 'appService'];

        constructor(
            $scope: ICollectionExplorerControllerScope,
            $http: ng.IHttpService,
            appService: AppService) {

            $scope.gridColumns = [];

            var collection: any = $scope.obj;

            for (var i = 0; i < collection.ColumnHeaders.length; i++) {
                var newColumn = {
                    name: collection.ColumnHeaders[i].Name,
                    field: collection.ColumnHeaders[i].PropertyName + ".Value"
                };
                $scope.gridColumns[i] = newColumn;
            }

            $scope.gridOptions = {
                enableSorting: true,
                columnDefs: $scope.gridColumns,
                data: collection.Items
            };
        }

    }

}
