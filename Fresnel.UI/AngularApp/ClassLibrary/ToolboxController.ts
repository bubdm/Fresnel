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
                    var result = promiseResult.data;

                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);

                    this.classHierarchy = promiseResult.data;
                });
            }

            $scope.create = function (fullyQualifiedName: string) {
                var promise = fresnelService.createObject(fullyQualifiedName);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;

                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);

                    if (result.Passed) {
                        var newObject = result.NewObject;
                        appService.identityMap.addObject(newObject);
                        $rootScope.$broadcast("openNewExplorer", newObject);
                    }
                });
            }

            $scope.getObjects = function (fullyQualifiedName: string) {
                var request = requestBuilder.buildGetObjectsRequest(fullyQualifiedName);

                var promise = fresnelService.getObjects(request);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;

                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);

                    if (result.Passed) {
                        appService.identityMap.addObject(result.Matches);
                        $rootScope.$broadcast("openNewExplorer", result.Matches);
                    }
                });
            }



            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });

        }

    }
}