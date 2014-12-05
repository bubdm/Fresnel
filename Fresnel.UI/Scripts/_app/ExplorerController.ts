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

        }
    }

}
