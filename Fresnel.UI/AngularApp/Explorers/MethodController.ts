/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />

module FresnelApp {

    export class MethodController {

        static $inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'appService',
            'explorerService',
            'requestBuilder',
            'explorer', 'method'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: IMethodControllerScope,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            requestBuilder: RequestBuilder,
            explorer: Explorer,
            method: any) {

            $scope.explorer = explorer;
            $scope.method = method;

            method.ParametersSetByUser = [];

            $scope.invoke = function (method: any) {
                var request = requestBuilder.buildMethodInvokeRequest(method);
                var promise = fresnelService.invokeMethod(request);

                promise.then((promiseResult) => {
                    var result = promiseResult.data;
                    method.Error = result.Passed ? "" : result.Messages[0].Text;

                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);

                    if (result.ResultObject) {
                        $rootScope.$broadcast("openNewExplorer", result.ResultObject);
                    }
                });
            }

            $scope.setProperty = function (param: any) {
                // Find the parameter, and set it's value:
                // If it's an Object, set the ReferenceValueID
                if (!param.IsNonReference) {
                    param.State.ReferenceValueID = param.State.Value.ID;
                }

                var matches = $.grep(method.ParametersSetByUser, function (e) {
                    return e == param;
                });
                if (matches.length == 0) {
                    method.ParametersSetByUser.push(param);
                }
            }

            $scope.setBitwiseEnumProperty = function (param: any, enumValue: number) {
                param.State.Value = param.State.Value ^ enumValue;
                $scope.setProperty(param);
            }

            $scope.cancel = function () {
                //modalInstance.dismiss();
            }

        }

    }
}
