module FresnelApp {

    export class SaveService {

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
            if (!obj.DirtyState.IsPersistent)
                return false;

            return (obj.DirtyState.IsTransient || obj.DirtyState.IsDirty || obj.DirtyState.HasDirtyChildren);
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

        public saveChanges(obj: ObjectVM): ng.IPromise<any> {
            var request = this.requestBuilder.buildSaveChangesRequest(obj);
            var promise = this.fresnelService.saveChanges(request);

            promise.then((promiseResult) => {
                var response: SaveChangesResponse = promiseResult.data;

                this.appService.identityMap.merge(response.Modifications);
                this.rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                this.resetDirtyFlags(response.SavedObjects);
            });

            return promise;
        }

        public cancelChanges(obj: ObjectVM): ng.IPromise<any> {
            var request = this.requestBuilder.buildCancelChangesRequest(obj);
            var promise = this.fresnelService.cancelChanges(request);

            promise.then((promiseResult) => {
                var response: CancelChangesResponse = promiseResult.data;

                this.appService.identityMap.merge(response.Modifications);
                this.rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                this.resetDirtyFlags(response.CancelledObjects);
            });

            return promise;
        }

        private resetDirtyFlags(savedObjects: ObjectVM[]) {
            if (!savedObjects)
                return;

            var identityMap = this.appService.identityMap;

            for (var i = 0; i < savedObjects.length; i++) {
                var obj = savedObjects[i];
                var existingObj = identityMap.getObject(obj.ID);
                if (existingObj != null) {
                    identityMap.mergeObjects(existingObj, obj);
                }
            }
        }

    }
}
