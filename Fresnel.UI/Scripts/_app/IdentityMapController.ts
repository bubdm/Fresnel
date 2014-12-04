module FresnelApp {


    export class IdentityMap {

        constructor($scope: IIdentityMapControllerScope) {

            $scope.items = [];

            $scope.add = function (key, value) {
                $scope.items.push({
                    key: key,
                    value: value
                });
            }

            $scope.remove = function (key) {
                var index = $scope.items.indexOf(key);
                if (index > -1) {
                    $scope.items.splice(index, 1);
                }
            }

            $scope.merge = function (delta: IdentityMapDelta) {


            }
        }

    }

    export class IdentityMapDelta {

        public newItems;

        public modifiedItems;

        public deletedItems;
    }

}