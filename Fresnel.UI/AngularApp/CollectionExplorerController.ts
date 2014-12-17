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
                //fieldValueSelector = 
                var newColumn = { name: collection.ColumnHeaders[i] };
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
