module FresnelApp {

    export class ExplorerController {

        static $inject = ['$scope', 'appService'];

        constructor(
            $scope: IExplorerControllerScope,
            appService: AppService) {

            $scope.openExplorers = [];

            $scope.$on('objectCreated', function (event, obj: IObjectVM) {
                $scope.openExplorers.push(obj);
            });

            $scope.minimise = function (obj: IObjectVM) {
                obj.IsMaximised = false;
            }

            $scope.maximise = function (obj: IObjectVM) {
                obj.IsMaximised = true;
            }

            $scope.close = function (obj: IObjectVM) {
                // TODO: Check for dirty status

                var index = $scope.openExplorers.indexOf(obj);
                if (index > -1) {
                    $scope.openExplorers.splice(index, 1);

                    // TODO: If the object is no longer in the UI, Let the server know that it can be GCed
                }
            }

        }
    }

}
