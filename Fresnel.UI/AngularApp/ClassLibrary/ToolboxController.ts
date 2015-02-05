module FresnelApp {

    export class ToolboxController {
        public classHierarchy: ClassItem[];

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
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
                    $rootScope.$broadcast("openNewExplorer", newObject);
                });
            }

            $scope.getObjects = function (fullyQualifiedName: string) {
                var request = requestBuilder.buildGetObjectsRequest(fullyQualifiedName);

                var promise = fresnelService.getObjects(request);

                promise.then((promiseResult) => {
                    var resultCollection = promiseResult.data.Results;
                    appService.identityMap.addObject(resultCollection);
                    $rootScope.$broadcast("openNewExplorer", resultCollection);
                });
            }



            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });

        }

    }
}