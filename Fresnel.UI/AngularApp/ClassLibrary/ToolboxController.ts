module FresnelApp {

    export class ToolboxController {
        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService',
            'methodInvoker'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IToolboxControllerScope,
            fresnelService: IFresnelService,
            requestBuilder: RequestBuilder,
            appService: AppService,
            searchService: SearchService,
            methodInvoker: MethodInvoker) {

            $scope.loadDomainLibrary = function () {
                var promise = fresnelService.getDomainLibrary();

                promise.then((promiseResult) => {
                    var response: GetDomainLibraryResponse = promiseResult.data;

                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    $scope.domainClassesHierarchy = response.DomainClasses;
                    $scope.domainServicesHierarchy = response.DomainServices;
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

            $scope.invokeDependencyMethod = function (method: MethodVM) {
                methodInvoker.invokeOrOpen(method);
            }

            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadDomainLibrary();
            });

        }

    }
}