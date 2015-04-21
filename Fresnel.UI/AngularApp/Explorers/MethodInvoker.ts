
module FresnelApp {

    export class MethodInvoker {

        static $inject = [
            '$rootScope',
            'fresnelService',
            'appService',
            'requestBuilder',
            '$modal'];

        private rootScope: ng.IRootScopeService;
        private fresnelService: IFresnelService;
        private appService: AppService;
        private requestBuilder: RequestBuilder;
        private modal: ng.ui.bootstrap.IModalService;

        constructor(
            $rootScope: ng.IRootScopeService,
            fresnelService: IFresnelService,
            appService: AppService,
            requestBuilder: RequestBuilder,
            $modal: ng.ui.bootstrap.IModalService) {

            this.rootScope = $rootScope;
            this.fresnelService = fresnelService;
            this.appService = appService;
            this.requestBuilder = requestBuilder;
            this.modal = $modal;
        }

        invokeOrOpen(method: MethodVM) {
            if (method.Parameters.length == 0) {
                var request = this.requestBuilder.buildInvokeMethodRequest(method);
                var promise = this.fresnelService.invokeMethod(request);

                promise.then((promiseResult) => {
                    var response = promiseResult.data;
                    method.Error = response.Passed ? "" : response.Messages[0].Text;

                    this.appService.identityMap.merge(response.Modifications);
                    this.rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                    if (response.ResultObject) {
                        this.rootScope.$broadcast(UiEventType.ExplorerOpen, response.ResultObject, null);
                    }
                });
            }
            else {
                var options: ng.ui.bootstrap.IModalSettings = {
                    templateUrl: '/Templates/methodDialog.html',
                    controller: 'methodController',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        // These objects will be injected into the MethodController's ctor:
                        explorer: function () {
                            var fakeExplorer = {
                                __meta: { ID: method.ObjectID },
                            };
                            return fakeExplorer;
                        },
                        method: function () {
                            return method;
                        }
                    }
                }

                var modal = this.modal.open(options);
                this.rootScope.$broadcast(UiEventType.ModalOpened, modal);

                modal.result.finally(() => {
                    this.rootScope.$broadcast(UiEventType.ModalClosed, modal);
                });
            }
        }
    }

}
