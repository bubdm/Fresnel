module FresnelApp {

    export class SaveCommand {

        static $inject = [
            '$rootScope',
            'appService',
            'fresnelService',
            'blockUI',
            '$modal',
            'requestBuilder'];

        private rootScope: ng.IRootScopeService;
        private appService: AppService;
        private fresnelService: IFresnelService;
        private blockUI: any;
        private modal: ng.ui.bootstrap.IModalService;
        private requestBuilder: RequestBuilder;

        constructor(
            rootScope: ng.IRootScopeService,
            appService: AppService,
            fresnelService: IFresnelService,
            blockUI: any,
            $modal: ng.ui.bootstrap.IModalService,
            requestBuilder: RequestBuilder
            ) {
            this.rootScope = rootScope;
            this.appService = appService;
            this.fresnelService = fresnelService;
            this.blockUI = blockUI;
            this.modal = $modal;
            this.requestBuilder = requestBuilder;
        }

        public isRequiredFor(obj: ObjectVM): boolean {
            if (!obj.IsPersistable)
                return false;

            return (obj.IsDirty || obj.HasDirtyChildren);
        }

        public askUser(obj: ObjectVM): ng.IPromise<any> {
            var modalOptions = this.createModalOptions();
            var modal = this.modal.open(modalOptions);
            this.rootScope.$broadcast(UiEventType.ModalOpened, modal);

            modal.result.finally(() => {
                this.rootScope.$broadcast(UiEventType.ModalClosed, modal);
            });

            return modal.result;
        }

        private createModalOptions(): ng.ui.bootstrap.IModalSettings {
            var options: ng.ui.bootstrap.IModalSettings = {
                templateUrl: '/Templates/saveDialog.html',
                controller: 'saveController',
                backdrop: 'static',
                size: 'sm'
            }

            return options;
        }

        public invoke(obj: ObjectVM): ng.IPromise<any> {
            var request = this.requestBuilder.buildSaveChangesRequest(obj);
            var promise = this.fresnelService.saveChanges(request);

            promise.then((promiseResult) => {
                var response = promiseResult.data;

                this.appService.identityMap.merge(response.Modifications);
                this.rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
            });

            return promise;
        }
    }

}