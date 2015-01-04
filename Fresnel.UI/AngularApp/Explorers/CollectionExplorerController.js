var FresnelApp;
(function (FresnelApp) {
    var CollectionExplorerController = (function () {
        function CollectionExplorerController($scope, $http, appService) {
            $scope.gridColumns = [];
            var collection = $scope.obj;
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
        CollectionExplorerController.$inject = ['$scope', '$http', 'appService'];
        return CollectionExplorerController;
    })();
    FresnelApp.CollectionExplorerController = CollectionExplorerController;
})(FresnelApp || (FresnelApp = {}));
//# sourceMappingURL=CollectionExplorerController.js.map