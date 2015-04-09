module FresnelApp {

    export class ToolboxController {
        public classHierarchy: Namespace[];

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService',
            'blockUI'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
            searchService: SearchService,
            blockUI: any) {

            $scope.loadClassHierarchy = function () {
                var promise = fresnelService.getClassHierarchy();

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    this.classHierarchy = promiseResult.data;
                });
            }

            $scope.create = function (fullyQualifiedName: string) {
                var request = requestBuilder.buildCreateObjectRequest(null, fullyQualifiedName);
                var promise = fresnelService.createObject(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    if (response.Passed) {
                        var newObject = response.NewObject;
                        appService.identityMap.addObject(newObject);
                        $rootScope.$broadcast(UiEventType.ExplorerOpen, newObject);
                    }
                });
            }

            $scope.searchObjects = function (fullyQualifiedName: string) {
                searchService.searchForObjects(fullyQualifiedName);
            }

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });

        }

    }
}