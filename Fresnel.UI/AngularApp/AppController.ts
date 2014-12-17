module FresnelApp {

    export class AppController {
        constructor(
            $scope: IAppControllerScope,
            appService: AppService) {

            appService.identityMap = new IdentityMap();

            $scope.identityMap = appService.identityMap;

        }
    }

}
