module FresnelApp {

    export class ExplorerController {

        static $inject = ['$scope', 'appService']; 

        constructor(
            $scope: IExplorerControllerScope,
            appService: AppService) {

            $scope.openExplorers = [];

            $scope.$on('objectCreated', function (event, data: IObjectVM) {
                var obj = appService.identityMap.items[data.ID];
                $scope.openExplorers.push[obj];
            });

        }
    }

}
