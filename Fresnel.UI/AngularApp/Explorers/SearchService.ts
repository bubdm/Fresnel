
module FresnelApp {

    export class SearchService {

        static $inject = [
            '$rootScope',
            'fresnelService',
            'appService',
            'explorerService',
            'requestBuilder',
            'blockUI',
            '$modal'];

        private rootScope: ng.IRootScopeService;
        private fresnelService: IFresnelService;
        private appService: AppService;
        private explorerService: ExplorerService;
        private requestBuilder: RequestBuilder;
        private blockUI: any;
        private modal: ng.ui.bootstrap.IModalService;

        constructor(
            $rootScope: ng.IRootScopeService,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            requestBuilder: RequestBuilder,
            blockUI: any,
            $modal: ng.ui.bootstrap.IModalService) {

            this.rootScope = $rootScope;
            this.fresnelService = fresnelService;
            this.appService = appService;
            this.explorerService = explorerService;
            this.requestBuilder = requestBuilder;
            this.blockUI = blockUI;
            this.modal = $modal;
        }

        showSearchForProperty(prop: PropertyVM, onSelectionConfirmed) {
            this.blockUI.start("Searching for data...");

            var request = this.requestBuilder.buildSearchPropertyRequest(prop);
            var searchPromise = this.fresnelService.searchPropertyObjects(request);

            searchPromise.then((promiseResult) => {
                var response = promiseResult.data;
                var searchResults: SearchResultsVM = response.Result;
                searchResults.OriginalRequest = request;
                searchResults.AllowSelection = true;
                searchResults.AllowMultiSelect = true;

                this.showSearchResultsModal(searchResults, onSelectionConfirmed);
            });
        }

        showSearchForParameter(method: MethodVM, param: ParameterVM, onSelectionConfirmed) {
            this.blockUI.start("Searching for data...");

            var request = this.requestBuilder.buildSearchParameterRequest(method, param);
            var searchPromise = this.fresnelService.searchParameterObjects(request);

            searchPromise.then((promiseResult) => {
                var response = promiseResult.data;
                var searchResults: SearchResultsVM = response.Result;
                searchResults.OriginalRequest = request;
                searchResults.AllowSelection = true;
                searchResults.AllowMultiSelect = true;
               
                this.showSearchResultsModal(searchResults, onSelectionConfirmed);
            });
        }

        private showSearchResultsModal(searchResults: SearchResultsVM, onSelectionConfirmed) {
            // Ensure that we re-use any objects that are already cached:
            var bindableItems = this.mergeSearchResults(searchResults);
            searchResults.Items = bindableItems;

            // This allows Smart-Table to handle the st-safe-src properly:
            searchResults.DisplayItems = [].concat(bindableItems);

            var searchExplorer = this.explorerService.addExplorer(searchResults);

            var modalOptions = this.createModalOptions(searchExplorer);
            var modal = this.modal.open(modalOptions);
            this.rootScope.$broadcast(UiEventType.ModalOpened, modal);

            this.blockUI.stop();

            modal.result.then(() => {
                var selectedItems = $.grep(searchResults.Items, function (o: SearchResultItemVM) {
                    return o.IsSelected;
                });
                if (selectedItems.length > 0) {
                    onSelectionConfirmed(selectedItems);
                }
            });

            modal.result.finally(() => {
                this.rootScope.$broadcast(UiEventType.ModalClosed, modal);
            });
        }

        private createModalOptions(searchExplorer: Explorer): ng.ui.bootstrap.IModalSettings {
            var options: ng.ui.bootstrap.IModalSettings = {
                templateUrl: '/Templates/searchResultsExplorer.html',
                controller: 'searchModalController',
                backdrop: 'static',
                size: 'lg',
                resolve: {
                    // These objects will be injected into the SearchController's ctor:
                    explorer: function () {
                        return searchExplorer;
                    }
                }
            }

            return options;
        }

        openNewExplorer(obj: ObjectVM, $rootScope: ng.IScope, parentExplorer: Explorer) {
            // As the collection only contains a lightweight object, we need to fetch one with more detail:
            var request = this.requestBuilder.buildGetObjectRequest(obj);
            var promise = this.fresnelService.getObject(request);

            promise.then((promiseResult) => {
                var response = promiseResult.data;

                this.appService.identityMap.merge(response.Modifications);
                $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);

                if (response.Passed) {
                    var latestObj = response.ReturnValue;
                    var existingObj = this.appService.identityMap.getObject(obj.ID);
                    if (existingObj == null) {
                        this.appService.identityMap.addObject(latestObj);
                    }
                    else {
                        this.appService.identityMap.mergeObjects(existingObj, latestObj);
                    }
                    $rootScope.$broadcast(UiEventType.ExplorerOpen, latestObj, parentExplorer);
                }
            });
        }

        loadNextPage(request: SearchRequest, existingSearchResults: SearchResultsVM, searchPromise: any) {
            request.PageNumber++;

            this.blockUI.start("Loading more data...");

            searchPromise().then((promiseResult) => {
                var response = promiseResult.data;
                var newSearchResults: SearchResultsVM = response.Result;
                if (newSearchResults.Items.length == 0)
                    return;

                // Ensure that we re-use any objects that are already cached:
                var bindableItems = this.mergeSearchResults(newSearchResults);

                // Append the new items to the exist results:
                for (var i = 0; i < bindableItems.length; i++) {
                    existingSearchResults.Items.push(bindableItems[i]);
                }

                // This allows Smart-Table to handle the st-safe-src properly:
                existingSearchResults.DisplayItems = [].concat(existingSearchResults.Items);
            })
                .finally(() => {
                this.blockUI.stop();
            });
        }

        mergeSearchResults(searchResults: SearchResultsVM): ObjectVM[] {
            var itemCount = searchResults.Items.length;
            var bindableItems: SearchResultItemVM[] = [itemCount];
            var identityMap = this.appService.identityMap;

            // If an object already exists in the IdentityMap we need to reuse it:
            for (var i = 0; i < itemCount; i++) {
                var latestObj = searchResults.Items[i];
                var existingObj = identityMap.getObject(latestObj.ID);

                var itemToBind = existingObj == null ? latestObj : existingObj;
                if (existingObj == null) {
                    identityMap.addObject(latestObj);
                    bindableItems[i] = latestObj;
                }
                else {
                    identityMap.mergeObjects(existingObj, latestObj);
                    bindableItems[i] = <SearchResultItemVM>existingObj;
                }

                bindableItems[i].IsSelected = false;
            }

            // Return the results, but now re-using any objects from the IdentityMap:
            return bindableItems;
        }

    }

}
