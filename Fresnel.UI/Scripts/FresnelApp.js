var FresnelApp;
(function (FresnelApp) {
    function ToolboxTreeNodeExpanderDirective() {
        return {
            link: function (scope, elem, attributes) {
                scope.$watchCollection('domainClassesHierarchy', function (newVal, oldVal) {
                    // This line is purely for VS "Find References", to remind Devs that this Directive 
                    // is associated with the collections:
                    var dummy = scope.domainClassesHierarchy;
                    // Force the treeview to register any new nodes:
                    $(".sidebar .treeview").tree();
                });
            }
        };
    }
    FresnelApp.ToolboxTreeNodeExpanderDirective = ToolboxTreeNodeExpanderDirective;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var ExplorerRow = (function () {
        function ExplorerRow() {
        }
        return ExplorerRow;
    })();
    FresnelApp.ExplorerRow = ExplorerRow;
})(FresnelApp || (FresnelApp = {}));
/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />
var FresnelApp;
(function (FresnelApp) {
    var SaveController = (function () {
        function SaveController($rootScope, $scope) {
            $scope.yes = function () {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal = $scope;
                modal.$close(true);
            };
            $scope.no = function () {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal = $scope;
                modal.$close(false);
            };
            $scope.cancel = function () {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal = $scope;
                modal.$dismiss();
            };
        }
        SaveController.$inject = [
            '$rootScope',
            '$scope'
        ];
        return SaveController;
    })();
    FresnelApp.SaveController = SaveController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var SaveService = (function () {
        function SaveService(rootScope, appService, fresnelService, blockUI, $modal, requestBuilder) {
            this.rootScope = rootScope;
            this.appService = appService;
            this.fresnelService = fresnelService;
            this.blockUI = blockUI;
            this.modal = $modal;
            this.requestBuilder = requestBuilder;
        }
        SaveService.prototype.isRequiredFor = function (obj) {
            if (!obj.IsPersistable)
                return false;
            return (obj.DirtyState.IsTransient || obj.DirtyState.IsDirty || obj.DirtyState.HasDirtyChildren);
        };
        SaveService.prototype.askUser = function (obj) {
            var _this = this;
            var modalOptions = this.createModalOptions();
            var modal = this.modal.open(modalOptions);
            this.rootScope.$broadcast(FresnelApp.UiEventType.ModalOpened, modal);
            modal.result.finally(function () {
                _this.rootScope.$broadcast(FresnelApp.UiEventType.ModalClosed, modal);
            });
            return modal.result;
        };
        SaveService.prototype.createModalOptions = function () {
            var options = {
                templateUrl: '/Templates/saveDialog.html',
                controller: 'saveController',
                backdrop: 'static',
                size: 'sm'
            };
            return options;
        };
        SaveService.prototype.saveChanges = function (obj) {
            var _this = this;
            var request = this.requestBuilder.buildSaveChangesRequest(obj);
            var promise = this.fresnelService.saveChanges(request);
            promise.then(function (promiseResult) {
                var response = promiseResult.data;
                _this.appService.identityMap.merge(response.Modifications);
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                _this.resetDirtyFlags(response.SavedObjects);
            });
            return promise;
        };
        SaveService.prototype.cancelChanges = function (obj) {
            var _this = this;
            var request = this.requestBuilder.buildCancelChangesRequest(obj);
            var promise = this.fresnelService.cancelChanges(request);
            promise.then(function (promiseResult) {
                var response = promiseResult.data;
                _this.appService.identityMap.merge(response.Modifications);
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                _this.resetDirtyFlags(response.CancelledObjects);
            });
            return promise;
        };
        SaveService.prototype.resetDirtyFlags = function (savedObjects) {
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
        };
        SaveService.$inject = [
            '$rootScope',
            'appService',
            'fresnelService',
            'blockUI',
            '$modal',
            'requestBuilder'
        ];
        return SaveService;
    })();
    FresnelApp.SaveService = SaveService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    // Used to control the interactions within an open Search form
    var SearchModalController = (function () {
        function SearchModalController($rootScope, $scope, fresnelService, searchService, explorer) {
            $scope.explorer = explorer;
            $scope.results = explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;
            $scope.searchAction = function () {
                return fresnelService.searchObjects($scope.request);
            };
            // TODO: Determine which FresnelService.Search() method to use:
            // $scope.searchPromise = fresnelService.SearchPropertyObjects($scope.request);
            // $scope.searchPromise = fresnelService.SearchParameterObjects($scope.request);
            $scope.openNewExplorer = function (obj) {
                searchService.openNewExplorer(obj, $rootScope, $scope.explorer);
            };
            $scope.loadNextPage = function () {
                searchService.loadNextPage($scope.request, $scope.results, $scope.searchAction);
            };
            $scope.close = function (explorer) {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal = $scope;
                modal.$dismiss();
            };
        }
        SearchModalController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'searchService',
            'explorer'
        ];
        return SearchModalController;
    })();
    FresnelApp.SearchModalController = SearchModalController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    // Used to control the interactions within an open Search form
    var SearchExplorerController = (function () {
        function SearchExplorerController($rootScope, $scope, fresnelService, searchService, smartTablePredicateService, appService, blockUI) {
            this.scope = $scope;
            $scope.isSearchVisible = false;
            $scope.results = $scope.explorer.__meta;
            $scope.request = $scope.results.OriginalRequest;
            $scope.results.AllowMultiSelect = false;
            $scope.searchAction = function () {
                return fresnelService.searchObjects($scope.request);
            };
            // This allows Smart-Table to handle the st-safe-src properly:
            $scope.results.DisplayItems = [].concat($scope.results.Items);
            $scope.openNewExplorer = function (obj) {
                searchService.openNewExplorer(obj, $rootScope, $scope.explorer);
            };
            $scope.stTablePipe = function (tableState) {
                // Sorting on an Object/Collection property doesn't make sense, so don't allow it:
                var sortProperty = smartTablePredicateService.getSortProperty(tableState, $scope.results.ElementProperties);
                if (sortProperty == null || !sortProperty.IsNonReference)
                    return;
                // Changing the order means we start from the beginning again:
                $scope.request.PageNumber = 1;
                $scope.request.OrderBy = sortProperty.InternalName;
                $scope.request.IsDescendingOrder = tableState.sort.reverse;
                blockUI.start("Sorting by " + sortProperty.Name + "...");
                $scope.searchAction().then(function (promiseResult) {
                    var response = promiseResult.data;
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    if (response.Passed) {
                        var newSearchResults = response.Result;
                        // Ensure that we re-use any objects that are already cached:
                        var bindableItems = searchService.mergeSearchResults(newSearchResults);
                        // This allows Smart-Table to handle the st-safe-src properly:
                        $scope.results.Items = bindableItems;
                        $scope.results.DisplayItems = [].concat(bindableItems);
                        tableState.pagination.numberOfPages = 50;
                    }
                }).finally(function () {
                    blockUI.stop();
                });
            };
            $scope.loadNextPage = function () {
                searchService.loadNextPage($scope.request, $scope.results, $scope.searchAction);
            };
            $scope.setProperty = function (prop) {
                // Update the search results immedately:
                $scope.applyFilters();
            };
            $scope.setBitwiseEnumProperty = function (prop, enumValue) {
                if (!prop.State.Value || prop.State.Value == null) {
                    // Set a default so we can filp the bits afterwards:
                    prop.State.Value = 0;
                }
                prop.State.Value = prop.State.Value ^ enumValue;
                // Update the search results immedately:
                $scope.applyFilters();
            };
            $scope.applyFilters = function () {
                var searchFilters = [];
                var searchResults = $scope.explorer.__meta;
                for (var i = 0; i < searchResults.ElementProperties.length; i++) {
                    var elementProperty = searchResults.ElementProperties[i];
                    if (elementProperty.State.Value) {
                        var newFilter = {
                            PropertyName: elementProperty.InternalName,
                            FilterValue: elementProperty.State.Value
                        };
                        searchFilters.push(newFilter);
                    }
                }
                $scope.request.SearchFilters = searchFilters;
                searchService.loadFilteredResults($scope.request, $scope.results, $scope.searchAction);
            };
            $scope.resetFilters = function () {
                var searchResults = $scope.explorer.__meta;
                for (var i = 0; i < searchResults.ElementProperties.length; i++) {
                    searchResults.ElementProperties[i].State.Value = null;
                }
            };
        }
        SearchExplorerController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'searchService',
            'smartTablePredicateService',
            'appService',
            'blockUI'
        ];
        return SearchExplorerController;
    })();
    FresnelApp.SearchExplorerController = SearchExplorerController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var MethodInvoker = (function () {
        function MethodInvoker($rootScope, fresnelService, appService, requestBuilder, $modal) {
            this.rootScope = $rootScope;
            this.fresnelService = fresnelService;
            this.appService = appService;
            this.requestBuilder = requestBuilder;
            this.modal = $modal;
        }
        MethodInvoker.prototype.invokeOrOpen = function (method) {
            var _this = this;
            if (method.Parameters.length == 0) {
                var request = this.requestBuilder.buildInvokeMethodRequest(method);
                var promise = this.fresnelService.invokeMethod(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    method.Error = response.Passed ? "" : response.Messages[0].Text;
                    _this.appService.identityMap.merge(response.Modifications);
                    _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    if (response.ResultObject) {
                        _this.rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, response.ResultObject, null);
                    }
                });
            }
            else {
                var options = {
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
                };
                var modal = this.modal.open(options);
                this.rootScope.$broadcast(FresnelApp.UiEventType.ModalOpened, modal);
                modal.result.finally(function () {
                    _this.rootScope.$broadcast(FresnelApp.UiEventType.ModalClosed, modal);
                });
            }
        };
        MethodInvoker.$inject = [
            '$rootScope',
            'fresnelService',
            'appService',
            'requestBuilder',
            '$modal'
        ];
        return MethodInvoker;
    })();
    FresnelApp.MethodInvoker = MethodInvoker;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var SmartTablePredicateService = (function () {
        function SmartTablePredicateService() {
        }
        SmartTablePredicateService.prototype.getSortProperty = function (tableState, collectionProperties) {
            var predicate = tableState.sort.predicate;
            if (predicate) {
                var index1 = predicate.indexOf("[");
                var index2 = predicate.indexOf("]");
                var propertyIndex = predicate.substr(index1 + 1, index2 - index1 - 1);
                var sortProp = collectionProperties[propertyIndex];
                return sortProp;
            }
            return null;
        };
        return SmartTablePredicateService;
    })();
    FresnelApp.SmartTablePredicateService = SmartTablePredicateService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var SearchService = (function () {
        function SearchService($rootScope, fresnelService, appService, explorerService, requestBuilder, blockUI, $modal) {
            this.rootScope = $rootScope;
            this.fresnelService = fresnelService;
            this.appService = appService;
            this.explorerService = explorerService;
            this.requestBuilder = requestBuilder;
            this.blockUI = blockUI;
            this.modal = $modal;
        }
        SearchService.prototype.searchForObjects = function (fullyQualifiedName) {
            var _this = this;
            var request = this.requestBuilder.buildSearchObjectsRequest(fullyQualifiedName);
            var promise = this.fresnelService.searchObjects(request);
            this.blockUI.start("Searching for data...");
            promise.then(function (promiseResult) {
                var response = promiseResult.data;
                _this.appService.identityMap.merge(response.Modifications);
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                if (response.Passed) {
                    var searchResults = response.Result;
                    searchResults.IsSearchResults = true;
                    searchResults.OriginalRequest = request;
                    searchResults.AllowSelection = false;
                    searchResults.AllowMultiSelect = false;
                    _this.appService.identityMap.addObject(response.Result);
                    _this.rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, response.Result);
                }
            }).finally(function () {
                _this.blockUI.stop();
            });
        };
        SearchService.prototype.showSearchForProperty = function (prop, onSelectionConfirmed) {
            var _this = this;
            this.blockUI.start("Searching for data...");
            var request = this.requestBuilder.buildSearchPropertyRequest(prop);
            var searchPromise = this.fresnelService.searchPropertyObjects(request);
            searchPromise.then(function (promiseResult) {
                var response = promiseResult.data;
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                if (response.Passed) {
                    var searchResults = response.Result;
                    searchResults.OriginalRequest = request;
                    searchResults.AllowSelection = true;
                    searchResults.AllowMultiSelect = true;
                    _this.showSearchResultsModal(searchResults, onSelectionConfirmed);
                }
            });
        };
        SearchService.prototype.showSearchForParameter = function (method, param, onSelectionConfirmed) {
            var _this = this;
            this.blockUI.start("Searching for data...");
            var request = this.requestBuilder.buildSearchParameterRequest(method, param);
            var searchPromise = this.fresnelService.searchParameterObjects(request);
            searchPromise.then(function (promiseResult) {
                var response = promiseResult.data;
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                if (response.Passed) {
                    var searchResults = response.Result;
                    searchResults.OriginalRequest = request;
                    searchResults.AllowSelection = true;
                    searchResults.AllowMultiSelect = true;
                    _this.showSearchResultsModal(searchResults, onSelectionConfirmed);
                }
            });
        };
        SearchService.prototype.showSearchResultsModal = function (searchResults, onSelectionConfirmed) {
            var _this = this;
            // Ensure that we re-use any objects that are already cached:
            var bindableItems = this.mergeSearchResults(searchResults);
            searchResults.Items = bindableItems;
            // This allows Smart-Table to handle the st-safe-src properly:
            searchResults.DisplayItems = [].concat(bindableItems);
            var searchExplorer = this.explorerService.addExplorer(searchResults);
            var modalOptions = this.createModalOptions(searchExplorer);
            var modal = this.modal.open(modalOptions);
            this.rootScope.$broadcast(FresnelApp.UiEventType.ModalOpened, modal);
            this.blockUI.stop();
            modal.result.then(function () {
                var selectedItems = $.grep(searchResults.Items, function (o) {
                    return o.IsSelected;
                });
                if (selectedItems.length > 0) {
                    onSelectionConfirmed(selectedItems);
                }
            });
            modal.result.finally(function () {
                _this.rootScope.$broadcast(FresnelApp.UiEventType.ModalClosed, modal);
            });
        };
        SearchService.prototype.createModalOptions = function (searchExplorer) {
            var options = {
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
            };
            return options;
        };
        SearchService.prototype.openNewExplorer = function (obj, $rootScope, parentExplorer) {
            var _this = this;
            // As the collection only contains a lightweight object, we need to fetch one with more detail:
            var request = this.requestBuilder.buildGetObjectRequest(obj);
            var promise = this.fresnelService.getObject(request);
            promise.then(function (promiseResult) {
                var response = promiseResult.data;
                _this.appService.identityMap.merge(response.Modifications);
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                if (response.Passed) {
                    var latestObj = response.ReturnValue;
                    var existingObj = _this.appService.identityMap.getObject(obj.ID);
                    if (existingObj == null) {
                        _this.appService.identityMap.addObject(latestObj);
                    }
                    else {
                        _this.appService.identityMap.mergeObjects(existingObj, latestObj);
                    }
                    _this.rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, latestObj, parentExplorer);
                }
            });
        };
        SearchService.prototype.loadNextPage = function (request, existingSearchResults, searchPromise) {
            var _this = this;
            request.PageNumber++;
            this.blockUI.start("Loading more data...");
            searchPromise().then(function (promiseResult) {
                var response = promiseResult.data;
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                if (response.Passed) {
                    var newSearchResults = response.Result;
                    if (newSearchResults.Items.length == 0)
                        return;
                    // Ensure that we re-use any objects that are already cached:
                    var bindableItems = _this.mergeSearchResults(newSearchResults);
                    for (var i = 0; i < bindableItems.length; i++) {
                        existingSearchResults.Items.push(bindableItems[i]);
                    }
                    // This allows Smart-Table to handle the st-safe-src properly:
                    existingSearchResults.DisplayItems = [].concat(existingSearchResults.Items);
                    existingSearchResults.AreMoreAvailable = newSearchResults.AreMoreAvailable;
                }
            }).finally(function () {
                _this.blockUI.stop();
            });
        };
        SearchService.prototype.loadFilteredResults = function (request, existingSearchResults, searchPromise) {
            var _this = this;
            this.blockUI.start("Filtering data...");
            searchPromise().then(function (promiseResult) {
                var response = promiseResult.data;
                _this.rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                if (response.Passed) {
                    var newSearchResults = response.Result;
                    // Ensure that we re-use any objects that are already cached:
                    var bindableItems = _this.mergeSearchResults(newSearchResults);
                    // Replace the existing items:
                    existingSearchResults.Items = bindableItems;
                    // This allows Smart-Table to handle the st-safe-src properly:
                    existingSearchResults.DisplayItems = [].concat(existingSearchResults.Items);
                    existingSearchResults.AreMoreAvailable = newSearchResults.AreMoreAvailable;
                }
            }).finally(function () {
                _this.blockUI.stop();
            });
        };
        /// Merges the Search Results into the IdentityMap, 
        /// and returns a list of Objects ready for binding to the view
        SearchService.prototype.mergeSearchResults = function (searchResults) {
            var itemCount = searchResults.Items.length;
            var bindableItems = [];
            var identityMap = this.appService.identityMap;
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
                    bindableItems[i] = existingObj;
                }
                bindableItems[i].IsSelected = false;
            }
            // Return the results, but now re-using any objects from the IdentityMap:
            return bindableItems;
        };
        SearchService.$inject = [
            '$rootScope',
            'fresnelService',
            'appService',
            'explorerService',
            'requestBuilder',
            'blockUI',
            '$modal'
        ];
        return SearchService;
    })();
    FresnelApp.SearchService = SearchService;
})(FresnelApp || (FresnelApp = {}));
/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />
var FresnelApp;
(function (FresnelApp) {
    var MethodController = (function () {
        function MethodController($rootScope, $scope, fresnelService, appService, explorerService, searchService, requestBuilder, explorer, method) {
            $scope.explorer = explorer;
            $scope.method = method;
            method.ParametersSetByUser = [];
            for (var i = 0; i < method.Parameters.length; i++) {
                var param = method.Parameters[i];
                param.State.Value = null;
                param.State.ReferenceValueID = null;
            }
            $scope.invoke = function (method) {
                var request = requestBuilder.buildInvokeMethodRequest(method);
                var promise = fresnelService.invokeMethod(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    method.Error = response.Passed ? "" : response.Messages[0].Text;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    if (response.ResultObject) {
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, response.ResultObject, $scope.explorer);
                    }
                });
            };
            $scope.setProperty = function (param) {
                // BUG: This is to prevent 'digest' model changes accidentally triggering server code:
                // See https://github.com/angular/angular.js/issues/9867
                if ($scope.$$phase == "$digest")
                    return;
                $scope.setParameterOnServer(param);
            };
            $scope.setBitwiseEnumProperty = function (param, enumValue) {
                param.State.Value = param.State.Value ^ enumValue;
                $scope.setProperty(param);
            };
            $scope.isBitwiseEnumPropertySet = function (param, enumValue) {
                return (param.State.Value & enumValue) != 0;
            };
            $scope.associate = function (param) {
                var onSelectionConfirmed = function (selectedItems) {
                    if (selectedItems.length == 1) {
                        var selectedItem = selectedItems[0];
                        param.State.ReferenceValueID = selectedItem.ID;
                        $scope.setParameterOnServer(param);
                    }
                };
                searchService.showSearchForParameter(method, param, onSelectionConfirmed);
            };
            $scope.disassociate = function (param) {
                param.State.ReferenceValueID = null;
                param.State.Value = null;
                $scope.setParameterOnServer(param);
            };
            $scope.setParameterOnServer = function (param) {
                var obj = $scope.explorer.__meta;
                var method = $scope.method;
                var request = requestBuilder.buildSetParameterRequest(obj, method, param);
                var promise = fresnelService.setParameter(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    param.Error = response.Passed ? "" : response.Messages[0].Text;
                    if (response.Passed) {
                        // Track which parameters have been set by the user:
                        var index = method.ParametersSetByUser.indexOf(param);
                        if (index > -1) {
                            method.ParametersSetByUser.splice(index, 1);
                        }
                        method.ParametersSetByUser.push(param);
                    }
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                });
            };
            $scope.addExistingItems = function (param, coll) {
                var onSelectionConfirmed = function (selectedItems) {
                    // TODO
                    //var request = requestBuilder.buildAddItemsRequest(coll, selectedItems);
                    //var promise = fresnelService.addItemsToCollection(request);
                    //promise.then((promiseResult) => {
                    //    var response = promiseResult.data;
                    //    appService.identityMap.merge(response.Modifications);
                    //    $rootScope.$broadcast(UiEventType.MessagesReceived, response.Messages);
                    //});
                };
                searchService.showSearchForParameter(method, param, onSelectionConfirmed);
            };
            $scope.close = function (explorer) {
                // The scope is automatically augmented with the $dismiss() method
                // See http://angular-ui.github.io/bootstrap/#/modal
                var modal = $scope;
                modal.$dismiss();
            };
        }
        MethodController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'appService',
            'explorerService',
            'searchService',
            'requestBuilder',
            'explorer',
            'method'
        ];
        return MethodController;
    })();
    FresnelApp.MethodController = MethodController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var WorkbenchController = (function () {
        function WorkbenchController($rootScope, $scope, fresnelService, appService, explorerService) {
            $scope.visibleRows = [];
            $scope.$on(FresnelApp.UiEventType.ExplorerOpen, function (event, obj, parentExplorer) {
                if (!obj)
                    return;
                // Re-use the existing object, so that any bindings aren't lost:
                var existingObj = appService.identityMap.getObject(obj.ID);
                if (existingObj != null) {
                    obj = existingObj;
                }
                else {
                    appService.identityMap.addObject(obj);
                }
                var newExplorer = explorerService.getExplorer(obj.ID);
                if (newExplorer == null) {
                    newExplorer = explorerService.addExplorer(obj);
                    // Determine which row to put the new Explorer in:
                    var explorerRow = (parentExplorer == undefined || parentExplorer == null) ? null : parentExplorer.ParentRow;
                    if (explorerRow == undefined || explorerRow == null) {
                        explorerRow = {
                            Explorers: [],
                            ColourIndex: $scope.visibleRows.length % 8
                        };
                        $scope.visibleRows.push(explorerRow);
                    }
                    // Determine the position of the new Explorer in it's row:
                    var parentPanelIndex = explorerRow.Explorers.indexOf(parentExplorer);
                    var isPanelInserted = false;
                    if (parentPanelIndex > -1) {
                        var insertIndex = parentPanelIndex + 1;
                        if (insertIndex != explorerRow.Explorers.length) {
                            explorerRow.Explorers.splice(insertIndex, 0, newExplorer);
                            isPanelInserted = true;
                        }
                    }
                    if (!isPanelInserted) {
                        // Shove it on the end:
                        explorerRow.Explorers.push(newExplorer);
                    }
                    newExplorer.ParentRow = explorerRow;
                    newExplorer.ParentExplorer = parentExplorer;
                }
            });
            $scope.$on(FresnelApp.UiEventType.ExplorerClose, function (event, explorer) {
                var parentRow = explorer.ParentRow;
                var panelIndex = parentRow.Explorers.indexOf(explorer);
                if (panelIndex > -1) {
                    parentRow.Explorers.splice(panelIndex, 1);
                    // Dispose of the Explorer:
                    explorerService.remove(explorer);
                    explorer.ParentRow = null;
                    explorer.ParentExplorer = null;
                    // Determine if the row needs to disappear:
                    if (parentRow.Explorers.length == 0) {
                        var rowIndex = $scope.visibleRows.indexOf(parentRow);
                        $scope.visibleRows.splice(rowIndex, 1);
                    }
                    // Clean up the Session if necessary:
                    if ($scope.visibleRows.length == 0) {
                        var promise = fresnelService.cleanupSession();
                        promise.then(function (promiseResult) {
                            var response = promiseResult.data;
                            appService.identityMap.reset();
                            $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                        });
                    }
                }
            });
        }
        WorkbenchController.$inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService'];
        return WorkbenchController;
    })();
    FresnelApp.WorkbenchController = WorkbenchController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var ExplorerService = (function () {
        function ExplorerService($templateCache) {
            this.explorers = [];
            this.templateCache = $templateCache;
        }
        ExplorerService.prototype.addExplorer = function (obj) {
            var explorer = {
                __meta: obj,
                ParentRow: null,
                ParentExplorer: null,
                IsMaximised: true,
                CustomTemplateUrl: null,
            };
            this.CheckForCustomTemplate(explorer);
            this.attachMembers(explorer);
            this.explorers[obj.ID] = explorer;
            return explorer;
        };
        ExplorerService.prototype.CheckForCustomTemplate = function (explorer) {
            var templateUrl = "/Customisations/" + explorer.__meta.Type + ".html";
            // We're using XMLHttpRequest so that failures don't propogate to angular-inform:
            var request = new XMLHttpRequest();
            request.open('HEAD', templateUrl, false);
            request.send();
            if (request.status == 200) {
                explorer.CustomTemplateUrl = templateUrl;
            }
            else {
                // This ensures that newer templates are picked up next time:
                this.templateCache.remove(templateUrl);
            }
        };
        ExplorerService.prototype.getExplorer = function (objID) {
            var result = this.explorers[objID];
            return result;
        };
        ExplorerService.prototype.remove = function (explorer) {
            var objID = explorer.__meta.ID;
            delete this.explorers[objID];
        };
        ExplorerService.prototype.attachMembers = function (explorer) {
            var obj = explorer.__meta;
            if (obj.Properties) {
                for (var i = 0; i < obj.Properties.length; i++) {
                    var prop = obj.Properties[i];
                    explorer[prop.InternalName] = prop;
                }
            }
            if (obj.Methods) {
                for (var i = 0; i < obj.Methods.length; i++) {
                    var method = obj.Methods[i];
                    explorer[method.InternalName] = method;
                }
            }
        };
        ExplorerService.$inject = ['$templateCache'];
        return ExplorerService;
    })();
    FresnelApp.ExplorerService = ExplorerService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var CollectionExplorerController = (function () {
        function CollectionExplorerController($rootScope, $scope, fresnelService, requestBuilder, appService, searchService) {
            var collection = $scope.explorer.__meta;
            // This allows Smart-Table to handle the st-safe-src properly:
            collection.DisplayItems = [].concat(collection.Items);
            // NB: This overrides the function in ExplorerController
            $scope.openNewExplorer = function (obj) {
                // As the collection only contains a lightweight object, we need to fetch one with more detail:
                var request = requestBuilder.buildGetObjectRequest(obj);
                var promise = fresnelService.getObject(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    if (response.Passed) {
                        var latestObj = response.ReturnValue;
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        if (existingObj == null) {
                            appService.identityMap.addObject(latestObj);
                        }
                        else {
                            appService.identityMap.mergeObjects(existingObj, latestObj);
                        }
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, latestObj, $scope.explorer);
                    }
                });
            };
            $scope.addNewItem = function (prop, itemType) {
                var request = {
                    ParentObjectID: prop.ObjectID,
                    CollectionPropertyName: prop.InternalName,
                    ElementTypeName: itemType
                };
                var promise = fresnelService.addNewItemToCollection(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    // This will cause the new object to appear in a new Explorer:
                    $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, response.AddedItem, $scope.explorer);
                });
            };
            $scope.addExistingItems = function (prop) {
                var onSelectionConfirmed = function (selectedItems) {
                    var request = requestBuilder.buildAddItemsRequest(prop, selectedItems);
                    var promise = fresnelService.addItemsToCollection(request);
                    promise.then(function (promiseResult) {
                        var response = promiseResult.data;
                        appService.identityMap.merge(response.Modifications);
                        $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    });
                };
                searchService.showSearchForProperty(prop, onSelectionConfirmed);
            };
            $scope.removeItem = function (prop, obj) {
                var request = {
                    ParentObjectID: prop.ObjectID,
                    CollectionPropertyName: prop.InternalName,
                    ElementID: obj.ID
                };
                var promise = fresnelService.removeItemFromCollection(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                });
            };
        }
        CollectionExplorerController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService'
        ];
        return CollectionExplorerController;
    })();
    FresnelApp.CollectionExplorerController = CollectionExplorerController;
})(FresnelApp || (FresnelApp = {}));
/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />
var FresnelApp;
(function (FresnelApp) {
    var ExplorerController = (function () {
        function ExplorerController($rootScope, $scope, fresnelService, requestBuilder, appService, searchService, explorerService, saveService, methodInvoker, $modal) {
            $scope.invoke = function (method) {
                methodInvoker.invokeOrOpen(method);
            };
            $scope.setProperty = function (prop) {
                // BUG: This is to prevent 'digest' model changes accidentally triggering server code:
                // See https://github.com/angular/angular.js/issues/9867
                if ($scope.$$phase == "$digest")
                    return;
                var request = requestBuilder.buildSetPropertyRequest(prop);
                var promise = fresnelService.setProperty(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    prop.Error = response.Passed ? "" : response.Messages[0].Text;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    var obj = appService.identityMap.getObject(prop.ObjectID);
                    obj.DirtyState.IsDirty = true;
                });
            };
            $scope.setBitwiseEnumProperty = function (prop, enumValue) {
                prop.State.Value = prop.State.Value ^ enumValue;
                $scope.setProperty(prop);
            };
            $scope.isBitwiseEnumPropertySet = function (prop, enumValue) {
                return (prop.State.Value & enumValue) != 0;
            };
            $scope.createAndAssociate = function (prop, classTypeName) {
                var request = requestBuilder.buildCreateAndAssociateRequest(prop, classTypeName);
                var promise = fresnelService.createAndSetProperty(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    prop.Error = response.Passed ? "" : response.Messages[0].Text;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    var obj = appService.identityMap.getObject(prop.ObjectID);
                    obj.DirtyState.IsDirty = true;
                    if (response.Passed) {
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, response.NewObject, $scope.explorer);
                    }
                });
            };
            $scope.associate = function (prop) {
                var onSelectionConfirmed = function (selectedItems) {
                    if (selectedItems.length == 1) {
                        var selectedItem = selectedItems[0];
                        prop.State.ReferenceValueID = selectedItem.ID;
                        // NB: We can't call the setProperty() function, as a digest is already running.
                        //     Hence the need to in-line the code here:
                        var request = requestBuilder.buildSetPropertyRequest(prop);
                        var promise = fresnelService.setProperty(request);
                        promise.then(function (promiseResult) {
                            var response = promiseResult.data;
                            prop.Error = response.Passed ? "" : response.Messages[0].Text;
                            appService.identityMap.merge(response.Modifications);
                            $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                            var obj = appService.identityMap.getObject(prop.ObjectID);
                            obj.DirtyState.IsDirty = true;
                        });
                    }
                };
                searchService.showSearchForProperty(prop, onSelectionConfirmed);
            };
            $scope.disassociate = function (prop) {
                // If we have a *transient* explorer already open for the property's value, 
                // we need to close it when the property is cleared (not point keeping unwanted objects on screen):
                var transientExplorer;
                if (prop.State.Value) {
                    var propertyObjectValue = appService.identityMap.getObject(prop.State.Value.ID);
                    if (propertyObjectValue.DirtyState.IsTransient) {
                        transientExplorer = explorerService.getExplorer(propertyObjectValue.ID);
                    }
                }
                prop.State.Value = null;
                prop.State.ReferenceValueID = null;
                var request = requestBuilder.buildSetPropertyRequest(prop);
                var promise = fresnelService.setProperty(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    prop.Error = response.Passed ? "" : response.Messages[0].Text;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    var obj = appService.identityMap.getObject(prop.ObjectID);
                    obj.DirtyState.IsDirty = true;
                    if (transientExplorer) {
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerClose, transientExplorer);
                    }
                });
            };
            $scope.refresh = function (explorer) {
                var request = requestBuilder.buildGetObjectRequest(explorer.__meta);
                var promise = fresnelService.getObject(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    var obj = response.ReturnValue;
                    var existingObj = appService.identityMap.getObject(obj.ID);
                    appService.identityMap.mergeObjects(existingObj, obj);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                });
            };
            $scope.minimise = function (explorer) {
                explorer.IsMaximised = false;
            };
            $scope.maximise = function (explorer) {
                explorer.IsMaximised = true;
            };
            $scope.close = function (explorer) {
                var obj = explorer.__meta;
                if (!saveService.isRequiredFor(obj)) {
                    // Just close the window:
                    $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerClose, explorer);
                    return;
                }
                // See what the user wants to do:
                var promise = saveService.askUser(obj);
                promise.then(function (isSaveRequested) {
                    if (isSaveRequested) {
                        promise = saveService.saveChanges(obj);
                    }
                    else {
                        promise = saveService.cancelChanges(obj);
                    }
                    promise.finally(function () {
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerClose, explorer);
                    });
                });
            };
            $scope.openNewExplorer = function (obj) {
                $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, obj, $scope.explorer);
            };
            $scope.openNewExplorerForProperty = function (prop) {
                var request = requestBuilder.buildGetPropertyRequest(prop);
                var promise = fresnelService.getProperty(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    var obj = response.ReturnValue;
                    if (obj) {
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        if (existingObj == null) {
                            appService.identityMap.addObject(obj);
                        }
                        else {
                            // Re-use the existing object, so that any bindings aren't lost:
                            obj = existingObj;
                        }
                        // TODO: Using obj.OuterProperty has a limitation. Two objects may refer to the same object.
                        //       So the OuterProperty needs to be attached to the Explorer, NOT the Object itself:
                        obj.OuterProperty = prop;
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, obj, $scope.explorer);
                    }
                });
            };
            $scope.save = function (obj) {
                saveService.saveChanges(obj);
            };
        }
        ExplorerController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService',
            'explorerService',
            'saveService',
            'methodInvoker',
            '$modal'
        ];
        return ExplorerController;
    })();
    FresnelApp.ExplorerController = ExplorerController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    // Used to ensure the Toolbox allows interaction with the Class nodes
    function ExplorerDirective($timeout, $location, $anchorScroll) {
        return {
            // Scroll to the new panel:
            link: function (scope, elem, attributes) {
                // We're using a delay so that the element is rendered before we inspect it.
                // The delay must be longer than the animation used to render the element (see animations.css):
                var delay = 501;
                // See http://stackoverflow.com/a/20156250/80369
                $timeout(function () {
                    var explorer = scope.explorer;
                    var bodyElem = $("body");
                    var workbenchElem = $("#workbench");
                    var explorerElem = $("#explorer_" + explorer.__meta.ID);
                    {
                        //var isExplorerWiderThanViewport = (explorerElem.width() > workbenchElem.innerWidth());
                        //var doesExplorerHangOffLeft = (explorerElem.position().left < workbenchElem.scrollLeft());
                        //var doesExplorerHangOffRight = (
                        //    (explorerElem.position().left + explorerElem.width()) >
                        //    (workbenchElem.scrollLeft() + workbenchElem.innerWidth())
                        //    );
                        //if (isExplorerWiderThanViewport) {
                        //    // The Explorer should be left-justified in the viewport:
                        //    var xPos = explorerElem.position().left - workbenchElem.offset().left;
                        //    workbenchElem.animate({ scrollLeft: xPos }, delay);
                        //}
                        //else if (doesExplorerHangOffLeft) {
                        //    // The Explorer should be left-justified in the viewport:
                        //    var xPos = explorerElem.position().left - workbenchElem.offset().left;
                        //    workbenchElem.animate({ scrollLeft: xPos }, delay);
                        //}
                        //else if (doesExplorerHangOffRight) {
                        //    // The Explorer should be right-justified in the viewport:
                        //    var xPos = explorerElem.position().left - workbenchElem.offset().left;
                        //    workbenchElem.animate({ scrollLeft: xPos }, delay);
                        //}
                        var xPos = explorerElem.position().left - workbenchElem.offset().left;
                        workbenchElem.animate({ scrollLeft: xPos }, delay);
                    }
                    {
                        //var isExplorerTallerThanViewport = (explorerElem.height() > workbenchElem.innerHeight());
                        //var doesExplorerHangOffTop = (explorerElem.position().top < workbenchElem.scrollTop());
                        //var doesExplorerHangOffBottom = (
                        //    (explorerElem.position().top + explorerElem.height()) >
                        //    (workbenchElem.scrollTop() + workbenchElem.innerHeight())
                        //    );
                        //if (isExplorerTallerThanViewport) {
                        //    // The Explorer should be top-justified in the viewport:
                        //    var yPos = explorerElem.position().top;
                        //    bodyElem.animate({ scrollTop: xPos }, delay);
                        //}
                        //else if (doesExplorerHangOffTop) {
                        //    // The Explorer should be top-justified in the viewport:
                        //    var yPos = explorerElem.position().top;
                        //    bodyElem.animate({ scrollTop: xPos }, delay);
                        //}
                        //else if (doesExplorerHangOffBottom) {
                        //    // The Explorer should be bottom-justified in the viewport:
                        //    var yPos = explorerElem.position().top - workbenchElem.offset().top;
                        //    bodyElem.animate({ scrollTop: xPos }, delay);
                        //}
                        var yPos = explorerElem.position().top - workbenchElem.offset().top;
                        bodyElem.animate({ scrollTop: yPos }, delay);
                    }
                }, delay);
            }
        };
    }
    FresnelApp.ExplorerDirective = ExplorerDirective;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var Explorer = (function () {
        function Explorer() {
        }
        return Explorer;
    })();
    FresnelApp.Explorer = Explorer;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var RequestBuilder = (function () {
        function RequestBuilder() {
        }
        RequestBuilder.prototype.buildCreateObjectRequest = function (obj, fullClassTypeName) {
            var request = {
                ParentObjectID: obj != null ? obj.ID : null,
                ClassTypeName: fullClassTypeName
            };
            return request;
        };
        RequestBuilder.prototype.buildSetParameterRequest = function (obj, method, param) {
            var request = {
                ObjectID: obj.ID,
                MethodName: method.InternalName,
                ParameterName: param.InternalName,
                NonReferenceValue: param.State.Value,
                ReferenceValueId: param.State.ReferenceValueID,
            };
            return request;
        };
        RequestBuilder.prototype.buildInvokeMethodRequest = function (method) {
            var request = {
                ObjectID: method.ObjectID,
                MethodName: method.InternalName,
                Parameters: this.buildParametersFrom(method)
            };
            return request;
        };
        RequestBuilder.prototype.buildParametersFrom = function (method) {
            var params = [];
            for (var i = 0; i < method.Parameters.length; i++) {
                var param = method.Parameters[i];
                var requestParam = {
                    InternalName: param.InternalName,
                    State: {
                        Value: param.State.Value,
                        ReferenceValueID: param.State.ReferenceValueID
                    }
                };
                params.push(requestParam);
            }
            return params;
        };
        RequestBuilder.prototype.buildCreateAndAssociateRequest = function (prop, classTypeName) {
            var request = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                ClassTypeName: classTypeName
            };
            return request;
        };
        RequestBuilder.prototype.buildSetPropertyRequest = function (prop) {
            var request = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                NonReferenceValue: prop.State.Value,
                ReferenceValueId: prop.State.ReferenceValueID
            };
            return request;
        };
        RequestBuilder.prototype.buildGetPropertyRequest = function (prop) {
            var request = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName
            };
            return request;
        };
        RequestBuilder.prototype.buildGetObjectRequest = function (obj) {
            var request = {
                ObjectID: obj.ID
            };
            return request;
        };
        RequestBuilder.prototype.buildSaveChangesRequest = function (obj) {
            var request = {
                ObjectID: obj.ID
            };
            return request;
        };
        RequestBuilder.prototype.buildCancelChangesRequest = function (obj) {
            var request = {
                ObjectID: obj.ID,
                PropertyName: null
            };
            return request;
        };
        RequestBuilder.prototype.buildSearchObjectsRequest = function (fullyQualifiedName) {
            var request = {
                SearchType: fullyQualifiedName,
                SearchFilters: null,
                OrderBy: null,
                IsDescendingOrder: false,
                PageNumber: 1,
                PageSize: 100
            };
            return request;
        };
        RequestBuilder.prototype.buildSearchPropertyRequest = function (prop) {
            var request = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                SearchFilters: null,
                OrderBy: null,
                IsDescendingOrder: false,
                PageNumber: 1,
                PageSize: 100
            };
            return request;
        };
        RequestBuilder.prototype.buildSearchParameterRequest = function (method, param) {
            var request = {
                ObjectID: method.ObjectID,
                MethodName: method.InternalName,
                ParameterName: param.InternalName,
                SearchFilters: null,
                OrderBy: null,
                IsDescendingOrder: false,
                PageNumber: 1,
                PageSize: 100
            };
            return request;
        };
        RequestBuilder.prototype.buildAddItemsRequest = function (collectionProp, itemsToAdd) {
            var elementIDs = itemsToAdd.map(function (o) {
                return o.ID;
            });
            var request = {
                ParentObjectID: collectionProp.ObjectID,
                CollectionPropertyName: collectionProp.InternalName,
                ElementIDs: elementIDs,
            };
            return request;
        };
        return RequestBuilder;
    })();
    FresnelApp.RequestBuilder = RequestBuilder;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var FresnelService = (function () {
        function FresnelService($http, blockUi) {
            this.http = $http;
            this.blockUI = blockUi;
        }
        FresnelService.prototype.getSession = function () {
            var _this = this;
            this.blockUI.start("Starting your session...");
            var uri = "api/Session/GetSession";
            var promise = this.http.get(uri);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.getDomainLibrary = function () {
            var _this = this;
            this.blockUI.start("Setting up Library...");
            var uri = "api/Toolbox/GetDomainLibrary";
            var promise = this.http.get(uri);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.createObject = function (request) {
            var _this = this;
            this.blockUI.start("Creating new object...");
            var uri = "api/Toolbox/Create";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.getObject = function (request) {
            var _this = this;
            this.blockUI.start("Retrieving object...");
            var uri = "api/Explorer/GetObject";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.getProperty = function (request) {
            var _this = this;
            this.blockUI.start("Loading property...");
            var uri = "api/Explorer/GetObjectProperty";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.createAndSetProperty = function (request) {
            var _this = this;
            this.blockUI.start("Creating new object...");
            var uri = "api/Explorer/CreateAndSetProperty";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.setProperty = function (request) {
            var _this = this;
            this.blockUI.start("Setting property value...");
            var uri = "api/Explorer/SetProperty";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.setParameter = function (request) {
            var _this = this;
            this.blockUI.start("Setting parameter value...");
            var uri = "api/Explorer/SetParameter";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.invokeMethod = function (request) {
            var _this = this;
            this.blockUI.start("Performing action...");
            var uri = "api/Explorer/InvokeMethod";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.addNewItemToCollection = function (request) {
            var _this = this;
            this.blockUI.start("Adding new item to collection...");
            var uri = "api/Explorer/AddNewItemToCollection";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.addItemsToCollection = function (request) {
            var _this = this;
            this.blockUI.start("Adding items to collection...");
            var uri = "api/Explorer/AddItemsToCollection";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.removeItemFromCollection = function (request) {
            var _this = this;
            this.blockUI.start("Removing item from collection...");
            var uri = "api/Explorer/RemoveItemFromCollection";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.cleanupSession = function () {
            var _this = this;
            this.blockUI.start("Cleaning up your workbench...");
            var uri = "api/Session/CleanUp";
            var promise = this.http.get(uri);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.saveChanges = function (request) {
            var _this = this;
            this.blockUI.start("Saving all changes...");
            var uri = "api/Explorer/SaveChanges";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.cancelChanges = function (request) {
            var _this = this;
            this.blockUI.start("Cancelling changes...");
            var uri = "api/Explorer/CancelChanges";
            var promise = this.http.post(uri, request);
            promise.finally(function () {
                _this.blockUI.stop();
            });
            return promise;
        };
        FresnelService.prototype.searchObjects = function (request) {
            var uri = "api/Toolbox/SearchObjects";
            var promise = this.http.post(uri, request);
            return promise;
        };
        FresnelService.prototype.searchPropertyObjects = function (request) {
            var uri = "api/Explorer/SearchPropertyObjects";
            var promise = this.http.post(uri, request);
            return promise;
        };
        FresnelService.prototype.searchParameterObjects = function (request) {
            var uri = "api/Explorer/SearchParameterObjects";
            var promise = this.http.post(uri, request);
            return promise;
        };
        FresnelService.$inject = ['$http', 'blockUI'];
        return FresnelService;
    })();
    FresnelApp.FresnelService = FresnelService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var AppController = (function () {
        function AppController($scope, fresnelService, appService, inform) {
            appService.identityMap = new FresnelApp.IdentityMap();
            $scope.identityMap = appService.identityMap;
            $scope.loadSession = function () {
                var promise = fresnelService.getSession();
                promise.then(function (promiseResult) {
                    var session = promiseResult.data;
                    $scope.session = session;
                });
            };
            $scope.$on(FresnelApp.UiEventType.MessagesReceived, function (event, messages) {
                if (messages == null)
                    return;
                appService.mergeMessages(messages, $scope.session);
                for (var i = 0; i < messages.length; i++) {
                    var message = messages[i];
                    var messageType = message.IsSuccess ? 'success' : message.IsInfo ? 'info' : message.IsWarning ? 'warning' : message.IsError ? 'danger' : 'default';
                    // Don't let the message automatically fade away:
                    var messageTtl = message.RequiresAcknowledgement ? 0 : 5000;
                    inform.add(message.Text, { ttl: messageTtl, type: messageType });
                }
            });
            $scope.$on(FresnelApp.UiEventType.ModalOpened, function (event) {
                $scope.IsModalVisible = true;
            });
            $scope.$on(FresnelApp.UiEventType.ModalClosed, function (event) {
                $scope.IsModalVisible = false;
            });
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });
        }
        AppController.$inject = ['$scope', 'fresnelService', 'appService', 'inform'];
        return AppController;
    })();
    FresnelApp.AppController = AppController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var AppService = (function () {
        function AppService() {
        }
        AppService.prototype.mergeMessages = function (messages, target) {
            for (var i = 0; i < messages.length; i++) {
                target.Messages.push(messages[i]);
            }
        };
        return AppService;
    })();
    FresnelApp.AppService = AppService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var ToolboxController = (function () {
        function ToolboxController($rootScope, $scope, fresnelService, requestBuilder, appService, searchService, methodInvoker) {
            $scope.loadDomainLibrary = function () {
                var promise = fresnelService.getDomainLibrary();
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    $scope.domainClassesHierarchy = response.DomainClasses;
                    $scope.domainServicesHierarchy = response.DomainServices;
                });
            };
            $scope.create = function (fullyQualifiedName) {
                var request = requestBuilder.buildCreateObjectRequest(null, fullyQualifiedName);
                var promise = fresnelService.createObject(request);
                promise.then(function (promiseResult) {
                    var response = promiseResult.data;
                    appService.identityMap.merge(response.Modifications);
                    $rootScope.$broadcast(FresnelApp.UiEventType.MessagesReceived, response.Messages);
                    if (response.Passed) {
                        var newObject = response.NewObject;
                        appService.identityMap.addObject(newObject);
                        $rootScope.$broadcast(FresnelApp.UiEventType.ExplorerOpen, newObject);
                    }
                });
            };
            $scope.searchObjects = function (fullyQualifiedName) {
                searchService.searchForObjects(fullyQualifiedName);
            };
            $scope.invokeDependencyMethod = function (method) {
                methodInvoker.invokeOrOpen(method);
            };
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadDomainLibrary();
            });
        }
        ToolboxController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'searchService',
            'methodInvoker'
        ];
        return ToolboxController;
    })();
    FresnelApp.ToolboxController = ToolboxController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var UiEventType = (function () {
        function UiEventType() {
        }
        UiEventType.MessagesReceived = 'MessagesReceived';
        UiEventType.ExplorerOpen = 'ExplorerOpen';
        UiEventType.ExplorerOpened = 'ExplorerOpened';
        UiEventType.ExplorerClose = 'ExplorerClose';
        UiEventType.ExplorerClosed = 'ExplorerClosed';
        UiEventType.ModalOpen = 'ModalOpen';
        UiEventType.ModalOpened = 'ModalOpened';
        UiEventType.ModalClose = 'ModalClose';
        UiEventType.ModalClosed = 'ModalClosed';
        return UiEventType;
    })();
    FresnelApp.UiEventType = UiEventType;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    // Taken from http://stackoverflow.com/a/25391043/80369
    function DisableAnchorDirective() {
        return {
            compile: function (tElement, tAttrs, transclude) {
                //Disable ngClick
                tAttrs["ngClick"] = ("ng-click", "!(" + tAttrs["aDisabled"] + ") && (" + tAttrs["ngClick"] + ")");
                //Toggle "disabled" to class when aDisabled becomes true
                return function (scope, iElement, iAttrs) {
                    scope.$watch(iAttrs["aDisabled"], function (newValue) {
                        if (newValue !== undefined) {
                            iElement.toggleClass("disabled", newValue);
                        }
                    });
                    //Disable href on click
                    iElement.on("click", function (e) {
                        if (scope.$eval(iAttrs["aDisabled"])) {
                            e.preventDefault();
                        }
                    });
                };
            }
        };
    }
    FresnelApp.DisableAnchorDirective = DisableAnchorDirective;
})(FresnelApp || (FresnelApp = {}));
/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
var FresnelApp;
(function (FresnelApp) {
    var IdentityMap = (function () {
        function IdentityMap() {
            this.objectMap = [];
        }
        IdentityMap.prototype.getObject = function (key) {
            var item = this.objectMap[key];
            return item;
        };
        IdentityMap.prototype.addObject = function (obj) {
            this.objectMap[obj.ID] = obj;
            // Optimisation: By default, the Properties don't have the ObjectID set
            // So we have to do it here:
            if (obj.Properties) {
                for (var i = 0; i < obj.Properties.length; i++) {
                    var prop = obj.Properties[i];
                    prop.ObjectID = obj.ID;
                }
            }
            var isCollection = obj.hasOwnProperty("IsCollection");
            if (isCollection) {
                var coll = obj;
                for (var i = 0; i < coll.Items.length; i++) {
                    var item = coll.Items[i];
                    this.objectMap[item.ID] = item;
                }
            }
        };
        IdentityMap.prototype.remove = function (objID) {
            delete this.objectMap[objID];
        };
        IdentityMap.prototype.merge = function (modifications) {
            if (modifications == null)
                return;
            this.mergeNewObjects(modifications);
            this.mergePropertyChanges(modifications);
            this.mergeObjectTitleChanges(modifications);
            this.mergeRemovals(modifications);
            this.mergeParameterChanges(modifications);
            this.mergeDirtyStateChanges(modifications);
        };
        IdentityMap.prototype.mergeNewObjects = function (modifications) {
            if (!modifications.NewObjects)
                return;
            for (var i = 0; i < modifications.NewObjects.length; i++) {
                var item = modifications.NewObjects[i];
                var existingItem = this.getObject(item.ID);
                if (existingItem == null) {
                    this.addObject(item);
                }
                else {
                    // Merge the objects:
                    this.extendDeep(existingItem, item);
                }
            }
            for (var i = 0; i < modifications.CollectionAdditions.length; i++) {
                var addition = modifications.CollectionAdditions[i];
                var collection = this.getObject(addition.CollectionId);
                var element = this.getObject(addition.ElementId);
                if (collection != null) {
                    collection.Items.push(element);
                }
            }
        };
        IdentityMap.prototype.mergeObjectTitleChanges = function (modifications) {
            if (!modifications.ObjectTitleChanges)
                return;
            for (var i = 0; i < modifications.ObjectTitleChanges.length; i++) {
                var titleChange = modifications.ObjectTitleChanges[i];
                var existingItem = this.getObject(titleChange.ObjectID);
                if (existingItem == null) {
                    continue;
                }
                existingItem.Name = titleChange.Title;
            }
        };
        IdentityMap.prototype.mergePropertyChanges = function (modifications) {
            if (!modifications.PropertyChanges)
                return;
            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange = modifications.PropertyChanges[i];
                var existingItem = this.getObject(propertyChange.ObjectID);
                if (existingItem == null) {
                    continue;
                }
                var prop = $.grep(existingItem.Properties, function (e) {
                    return e.InternalName == propertyChange.PropertyName;
                }, false)[0];
                this.extendDeep(prop.State, propertyChange.State);
                // Finally, we can set the property value:
                var newPropertyValue = null;
                if (propertyChange.State.ReferenceValueID != null) {
                    newPropertyValue = this.getObject(propertyChange.State.ReferenceValueID);
                }
                else {
                    newPropertyValue = propertyChange.State.Value;
                }
                prop.State.Value = newPropertyValue;
            }
        };
        IdentityMap.prototype.mergeRemovals = function (modifications) {
            if (!modifications.CollectionRemovals)
                return;
            for (var i = 0; i < modifications.CollectionRemovals.length; i++) {
                var removal = modifications.CollectionRemovals[i];
                var collection = this.getObject(removal.CollectionId);
                var element = this.getObject(removal.ElementId);
                if (collection != null) {
                    var index = collection.Items.indexOf(element);
                    if (index > -1) {
                        collection.Items.splice(index, 1);
                    }
                }
            }
        };
        IdentityMap.prototype.mergeParameterChanges = function (modifications) {
            if (!modifications.MethodParameterChanges)
                return;
            for (var i = 0; i < modifications.MethodParameterChanges.length; i++) {
                var parameterChange = modifications.MethodParameterChanges[i];
                var existingItem = this.getObject(parameterChange.ObjectID);
                if (existingItem == null) {
                    continue;
                }
                var method = $.grep(existingItem.Methods, function (e) {
                    return e.InternalName == parameterChange.MethodName;
                }, false)[0];
                var param = $.grep(method.Parameters, function (e) {
                    return e.InternalName == parameterChange.ParameterName;
                }, false)[0];
                this.extendDeep(param.State, parameterChange.State);
                // Finally, we can set the parameter value:
                var paramValue = null;
                if (parameterChange.State.ReferenceValueID != null) {
                    paramValue = this.getObject(parameterChange.State.ReferenceValueID);
                }
                else {
                    paramValue = parameterChange.State.Value;
                }
                param.State.Value = paramValue;
            }
        };
        IdentityMap.prototype.mergeDirtyStateChanges = function (modifications) {
            if (!modifications.DirtyStateChanges)
                return;
            for (var i = 0; i < modifications.DirtyStateChanges.length; i++) {
                var dirtyState = modifications.DirtyStateChanges[i];
                var existingItem = this.getObject(dirtyState.ObjectID);
                if (existingItem == null) {
                    continue;
                }
                this.extendDeep(existingItem.DirtyState, dirtyState);
            }
        };
        IdentityMap.prototype.mergeObjects = function (existingObj, newObj) {
            var doesExistingObjHaveProperties = (existingObj.Properties != null) && (existingObj.Properties.length > 0);
            var doesNewObjHaveProperties = (newObj.Properties != null) && (newObj.Properties.length > 0);
            var doObjectsHaveSameProperties = doesExistingObjHaveProperties && doesNewObjHaveProperties && (existingObj.Properties.length == newObj.Properties.length);
            if (!doesExistingObjHaveProperties && doesNewObjHaveProperties) {
                existingObj.Properties = newObj.Properties;
            }
            else if (doObjectsHaveSameProperties) {
                for (var i = 0; i < existingObj.Properties.length; i++) {
                    this.extendDeep(existingObj.Properties[i], newObj.Properties[i]);
                }
            }
            var doesExistingObjHaveMethods = (existingObj.Methods != null) && (existingObj.Methods.length > 0);
            var doesNewObjHaveMethods = (newObj.Methods != null) && (newObj.Methods.length > 0);
            var doObjectsHaveSameMethods = doesExistingObjHaveMethods && doesNewObjHaveMethods && (existingObj.Methods.length == newObj.Methods.length);
            if (!doesExistingObjHaveMethods && doesNewObjHaveMethods) {
                existingObj.Methods = newObj.Methods;
            }
            else if (doObjectsHaveSameMethods) {
                for (var i = 0; i < existingObj.Methods.length; i++) {
                    this.extendDeep(existingObj.Methods[i], newObj.Methods[i]);
                }
            }
            if (!existingObj.DirtyState && newObj.DirtyState) {
                existingObj.DirtyState == newObj.DirtyState;
            }
            else {
                this.extendDeep(existingObj.DirtyState, newObj.DirtyState);
            }
        };
        IdentityMap.prototype.extendDeep = function (destination, source) {
            for (var property in source) {
                if (source[property] && source[property].constructor && source[property].constructor === Object) {
                    destination[property] = destination[property] || {};
                    arguments.callee(destination[property], source[property]);
                }
                else {
                    destination[property] = source[property];
                }
            }
            return destination;
        };
        IdentityMap.prototype.reset = function () {
            this.objectMap = [];
        };
        return IdentityMap;
    })();
    FresnelApp.IdentityMap = IdentityMap;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    function ToolboxTreeNodeTooltipDirective() {
        return {
            link: function (scope, elem, attributes) {
                scope.$watchCollection('domainClassesHierarchy', function (newVal, oldVal) {
                    // This line is purely for VS "Find References", to remind Devs that this Directive 
                    // is associated with the collections:
                    var dummy = scope.domainClassesHierarchy;
                    // Force the tooltips to respond:
                    $("[data-toggle='tooltip']").tooltip();
                });
            }
        };
    }
    FresnelApp.ToolboxTreeNodeTooltipDirective = ToolboxTreeNodeTooltipDirective;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var requires = ['blockUI', 'inform', 'inform-exception', 'inform-http-exception', 'ngAnimate', 'smart-table', 'ui.bootstrap'];
    angular.module("fresnelApp", requires).service("appService", FresnelApp.AppService).service("explorerService", FresnelApp.ExplorerService).service("fresnelService", FresnelApp.FresnelService).service("requestBuilder", FresnelApp.RequestBuilder).service("searchService", FresnelApp.SearchService).service("smartTablePredicateService", FresnelApp.SmartTablePredicateService).service("saveService", FresnelApp.SaveService).service("methodInvoker", FresnelApp.MethodInvoker).controller("appController", FresnelApp.AppController).controller("toolboxController", FresnelApp.ToolboxController).controller("workbenchController", FresnelApp.WorkbenchController).controller("explorerController", FresnelApp.ExplorerController).controller("methodController", FresnelApp.MethodController).controller("collectionExplorerController", FresnelApp.CollectionExplorerController).controller("searchExplorerController", FresnelApp.SearchExplorerController).controller("searchModalController", FresnelApp.SearchModalController).controller("saveController", FresnelApp.SaveController).directive("toolboxTreeNodeExpander", FresnelApp.ToolboxTreeNodeExpanderDirective).directive("toolboxTreeNodeTooltip", FresnelApp.ToolboxTreeNodeTooltipDirective).directive("objectExplorer", FresnelApp.ExplorerDirective).directive("aDisabled", FresnelApp.DisableAnchorDirective).config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.transformResponse.push(function (responseData) {
            convertDateStringsToDates(responseData);
            return responseData;
        });
    }]).config(function (blockUIConfig) {
        blockUIConfig.message = 'Please wait...';
        blockUIConfig.delay = 250;
        blockUIConfig.resetOnException = true;
        blockUIConfig.autoBlock = false;
    });
    // See http://aboutcode.net/2013/07/27/json-date-parsing-angularjs.html
    // and http://stackoverflow.com/a/8270148/80369
    // TODO: Refactor the Date conversion into a self-contained object:
    var regexIso8601 = /^(\d{4}\-\d\d\-\d\d([tT][\d:\.]*)?)([zZ]|([+\-])(\d\d):?(\d\d))?$/;
    function convertDateStringsToDates(input) {
        // Ignore things that aren't objects.
        if (typeof input !== "object")
            return input;
        for (var key in input) {
            if (!input.hasOwnProperty(key))
                continue;
            var value = input[key];
            var match;
            // Check for string properties which look like dates.
            if (typeof value === "string" && (match = value.match(regexIso8601))) {
                var milliseconds = Date.parse(match[0]);
                if (!isNaN(milliseconds)) {
                    input[key] = new Date(milliseconds);
                }
            }
            else if (typeof value === "object") {
                // Recurse into object
                convertDateStringsToDates(value);
            }
        }
    }
})(FresnelApp || (FresnelApp = {}));
