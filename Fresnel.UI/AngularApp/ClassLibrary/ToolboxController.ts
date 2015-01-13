module FresnelApp {

    export class ToolboxController {
        public classHierarchy: any;

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            fresnelService: IFresnelService,
            appService: AppService) {

            $scope.loadClassHierarchy = function () {
                var promise = fresnelService.getClassHierarchy();

                promise.then((promiseResult) => {
                    this.classHierarchy = promiseResult.data;
                });
            }

            $scope.create = function (fullyQualifiedName: string) {
                var promise = fresnelService.createObject(fullyQualifiedName);

                promise.then((promiseResult) => {
                    var newObject = promiseResult.data.NewObject;
                    appService.identityMap.addObject(newObject);
                    $rootScope.$broadcast("objectCreated", newObject);
                });
            }

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });

        }

    }
}