module FresnelApp {

    export class ToolboxController {
        public classHierarchy: Namespace[];

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'blockUI'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
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
                var promise = fresnelService.createObject(fullyQualifiedName);

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
                var request = requestBuilder.buildSearchObjectsRequest(fullyQualifiedName);
                var promise = fresnelService.searchObjects(request);

                blockUI.start("Searching for data...");

                promise.then((promiseResult) => {
                    var response = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    if (response.Passed) {
                        var searchResults: SearchResultsVM = response.Result;

                        searchResults.IsSearchResults = true;
                        searchResults.OriginalRequest = request;
                        searchResults.AllowSelection = false;
                        searchResults.AllowMultiSelect = false;

                        appService.identityMap.addObject(response.Result);
                        $rootScope.$broadcast(UiEventType.ExplorerOpen, response.Result);
                    }
                })
                    .finally(() => {
                    blockUI.stop();
                });
            }

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });

        }

    }
}