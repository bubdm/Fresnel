
module FresnelApp {

    export class SearchService {

        static $inject = [
            '$rootScope',
            'fresnelService',
            'appService',
            'explorerService',
            'requestBuilder',
            '$modal'];

        private rootScope: ng.IRootScopeService;
        private fresnelService: IFresnelService;
        private appService: AppService;
        private explorerService: ExplorerService;
        private requestBuilder: RequestBuilder;
        private modal: ng.ui.bootstrap.IModalService;

        constructor(
            $rootScope: ng.IRootScopeService,
            fresnelService: IFresnelService,
            appService: AppService,
            explorerService: ExplorerService,
            requestBuilder: RequestBuilder,
            $modal: ng.ui.bootstrap.IModalService) {

            this.rootScope = $rootScope;
            this.fresnelService = fresnelService;
            this.appService = appService;
            this.explorerService = explorerService;
            this.requestBuilder = requestBuilder;
            this.modal = $modal;
        }

        showSearchForCollection(coll: CollectionVM, onSelectionConfirmed) {
            var request = this.requestBuilder.buildSearchObjectsRequest(coll.ElementType);
            var searchPromise = this.fresnelService.searchObjects(request);

            // TODO: Open the modal _before_ the search is executed:

            searchPromise.then((promiseResult) => {
                var response = promiseResult.data;
                var searchResults: SearchResultsVM = response.Result;
                searchResults.OriginalRequest = request;
                searchResults.AllowMultiSelect = true;
                var searchExplorer = this.explorerService.addExplorer(searchResults);

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

                var modal = this.modal.open(options);
                this.rootScope.$broadcast(UiEventType.ModalOpened, modal);

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
            });
        }

        showSearchForProperty(prop: PropertyVM, onSelectionConfirmed) {
            var request = this.requestBuilder.buildSearchPropertyRequest(prop);
            var searchPromise = this.fresnelService.searchPropertyObjects(request);

            // TODO: Open the modal _before_ the search is executed:

            searchPromise.then((promiseResult) => {
                var response = promiseResult.data;
                var searchResults: SearchResultsVM = response.Result;
                searchResults.AllowMultiSelect = prop.IsCollection;
                searchResults.OriginalRequest = request;
                var searchExplorer = this.explorerService.addExplorer(searchResults);

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

                var modal = this.modal.open(options);
                this.rootScope.$broadcast(UiEventType.ModalOpened, modal);

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
            });
 }

        showSearchForParameter(param: ParameterVM, onSelectionConfirmed) {
        }

        openNewExplorer(obj: ObjectVM, $rootScope: ng.IScope) {

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
                    this.appService.identityMap.mergeObjects(existingObj, latestObj);
                    $rootScope.$broadcast(UiEventType.ExplorerOpen, latestObj);
                }
            });
        }

        loadNextPage(request: SearchRequest, results: SearchResultsVM, searchPromise: any) {
            request.PageNumber++;

            searchPromise().then((promiseResult) => {
                var response = promiseResult.data;
                var newSearchResults: SearchResultsVM = response.Result;

                // Append the new items to the exist results:
                var existingSearchResults = results;

                for (var i = 0; i < newSearchResults.Items.length; i++) {
                    existingSearchResults.Items.push(newSearchResults.Items[i]);
                }
            });
        }

    }

}
