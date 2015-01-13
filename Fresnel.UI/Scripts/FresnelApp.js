var FresnelApp;
(function (FresnelApp) {
    var ExplorerService = (function () {
        function ExplorerService() {
            this.explorers = [];
        }
        ExplorerService.prototype.addExplorer = function (obj) {
            var explorer = new FresnelApp.Explorer();
            explorer.__meta = obj;
            this.attachMembers(explorer);
            this.explorers[obj.ID] = explorer;
            return explorer;
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
                    explorer[prop.PropertyName] = prop;
                }
            }
            if (obj.Methods) {
                for (var i = 0; i < obj.Methods.length; i++) {
                    var method = obj.Methods[i];
                    explorer[method.MethodName] = method;
                }
            }
        };
        return ExplorerService;
    })();
    FresnelApp.ExplorerService = ExplorerService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var CollectionExplorerController = (function () {
        function CollectionExplorerController($rootScope, $scope, fresnelService, appService) {
            $scope.gridColumns = [];
            var collection = $scope.explorer.__meta;
            for (var i = 0; i < collection.ColumnHeaders.length; i++) {
                var newColumn = {
                    name: collection.ColumnHeaders[i].Name,
                    field: 'Properties[' + i + "].Value"
                };
                $scope.gridColumns[i] = newColumn;
            }
            $scope.gridOptions = {
                enableSorting: true,
                columnDefs: $scope.gridColumns,
                data: collection.Items
            };
            $scope.addNewItem = function (itemType) {
                var promise = fresnelService.createObject(itemType);
                promise.then(function (promiseResult) {
                    var newObject = promiseResult.data.NewObject;
                    appService.identityMap.addObject(newObject);
                    collection.Items.push(newObject);
                    // This will cause the new object to appear in a new Explorer:
                    //$rootScope.$broadcast("objectCreated", newObject);             
                });
            };
        }
        CollectionExplorerController.$inject = ['$rootScope', '$scope', 'fresnelService', 'appService'];
        return CollectionExplorerController;
    })();
    FresnelApp.CollectionExplorerController = CollectionExplorerController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var ObjectExplorerController = (function () {
        function ObjectExplorerController($rootScope, $scope, fresnelService, appService, explorerService) {
            $scope.visibleExplorers = [];
            $scope.$on('objectCreated', function (event, obj) {
                var explorer = explorerService.getExplorer(obj.ID);
                if (explorer == null) {
                    explorer = explorerService.addExplorer(obj);
                    $scope.visibleExplorers.push(explorer);
                }
            });
            $scope.invoke = function (method) {
                var promise = fresnelService.invokeMethod(method);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    method.Error = result.Passed ? "" : result.Messages.Errors[0].Text;
                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                });
            };
            $scope.setProperty = function (prop) {
                var request = {
                    ObjectId: prop.ObjectID,
                    PropertyName: prop.PropertyName,
                    NonReferenceValue: prop.Value
                };
                var promise = fresnelService.setProperty(request);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    prop.Error = result.Passed ? "" : result.Messages.Errors[0].Text;
                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                });
            };
            $scope.minimise = function (explorer) {
                explorer.IsMaximised = false;
            };
            $scope.maximise = function (explorer) {
                explorer.IsMaximised = true;
            };
            $scope.close = function (explorer) {
                // TODO: Check for dirty status
                var index = $scope.visibleExplorers.indexOf(explorer);
                if (index > -1) {
                    $scope.visibleExplorers.splice(index, 1);
                    explorerService.remove(explorer);
                }
            };
            $scope.openNewExplorer = function (prop) {
                var promise = fresnelService.getProperty(prop);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    var obj = result.ReturnValue;
                    if (obj) {
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        if (existingObj == null) {
                            appService.identityMap.addObject(obj);
                        }
                        obj.OuterProperty = prop;
                        // TODO: Insert the object just after it's parent?
                        var explorer = explorerService.getExplorer(obj.ID);
                        if (explorer == null) {
                            explorer = explorerService.addExplorer(obj);
                            $scope.visibleExplorers.push(explorer);
                        }
                    }
                });
            };
        }
        ObjectExplorerController.$inject = ['$rootScope', '$scope', 'fresnelService', 'appService', 'explorerService'];
        return ObjectExplorerController;
    })();
    FresnelApp.ObjectExplorerController = ObjectExplorerController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    // Used to ensure the Toolbox allows interaction with the Class nodes
    function ObjectExplorerDirective() {
        return {
            link: function (scope, elem, attributes) {
                scope.$watchCollection('visibleExplorers', function (newVal, oldVal) {
                    ////bootstrap WYSIHTML5 - text editor
                    //$(".textarea").wysihtml5();
                });
            }
        };
    }
    FresnelApp.ObjectExplorerDirective = ObjectExplorerDirective;
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
    var FresnelService = (function () {
        function FresnelService($http) {
            this.http = $http;
        }
        FresnelService.prototype.getSession = function () {
            var uri = "api/Session/GetSession";
            return this.http.get(uri);
        };
        FresnelService.prototype.getClassHierarchy = function () {
            var uri = "api/Toolbox/GetClassHierarchy";
            return this.http.get(uri);
        };
        FresnelService.prototype.createObject = function (fullyQualifiedName) {
            var uri = "api/Toolbox/Create";
            var arg = "=" + fullyQualifiedName;
            var config = {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }
            };
            return this.http.post(uri, arg, config);
        };
        FresnelService.prototype.getProperty = function (request) {
            var uri = "api/Explorer/GetObjectProperty";
            return this.http.post(uri, request);
        };
        FresnelService.prototype.setProperty = function (request) {
            var uri = "api/Explorer/SetProperty";
            return this.http.post(uri, request);
        };
        FresnelService.prototype.invokeMethod = function (request) {
            var uri = "api/Explorer/InvokeMethod";
            return this.http.post(uri, request);
        };
        FresnelService.$inject = ['$http'];
        return FresnelService;
    })();
    FresnelApp.FresnelService = FresnelService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var AppController = (function () {
        function AppController($scope, fresnelService, appService) {
            appService.identityMap = new FresnelApp.IdentityMap();
            $scope.identityMap = appService.identityMap;
            $scope.loadSession = function () {
                var promise = fresnelService.getSession();
                promise.then(function (promiseResult) {
                    var session = promiseResult.data;
                    $scope.session = session;
                });
            };
            $scope.$on('messagesReceived', function (event, messageSet) {
                $scope.session.TestValue++;
                appService.mergeMessages(messageSet, $scope.session);
            });
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });
        }
        AppController.$inject = ['$scope', 'fresnelService', 'appService'];
        return AppController;
    })();
    FresnelApp.AppController = AppController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var AppService = (function () {
        function AppService() {
        }
        AppService.prototype.mergeMessages = function (messageSet, target) {
            this.mergeMessageArray(messageSet.Infos, target.Messages.Infos);
            this.mergeMessageArray(messageSet.Warnings, target.Messages.Warnings);
            this.mergeMessageArray(messageSet.Errors, target.Messages.Errors);
        };
        AppService.prototype.mergeMessageArray = function (sourceArray, targetArray) {
            if (sourceArray) {
                for (var i = 0; i < sourceArray.length; i++) {
                    var message = sourceArray[i];
                    targetArray.push(message);
                }
            }
        };
        return AppService;
    })();
    FresnelApp.AppService = AppService;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var ToolboxController = (function () {
        function ToolboxController($rootScope, $scope, fresnelService, appService) {
            $scope.loadClassHierarchy = function () {
                var _this = this;
                var promise = fresnelService.getClassHierarchy();
                promise.then(function (promiseResult) {
                    _this.classHierarchy = promiseResult.data;
                });
            };
            $scope.create = function (fullyQualifiedName) {
                var promise = fresnelService.createObject(fullyQualifiedName);
                promise.then(function (promiseResult) {
                    var newObject = promiseResult.data.NewObject;
                    appService.identityMap.addObject(newObject);
                    $rootScope.$broadcast("objectCreated", newObject);
                });
            };
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });
        }
        ToolboxController.$inject = ['$rootScope', '$scope', 'fresnelService', 'appService'];
        return ToolboxController;
    })();
    FresnelApp.ToolboxController = ToolboxController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var Session = (function () {
        function Session() {
        }
        return Session;
    })();
    FresnelApp.Session = Session;
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
            this.remove(obj.ID);
            this.objectMap[obj.ID] = obj;
        };
        IdentityMap.prototype.remove = function (objID) {
            delete this.objectMap[objID];
        };
        IdentityMap.prototype.merge = function (modifications) {
            if (modifications == null)
                return;
            for (var i = 0; i < modifications.NewObjects.length; i++) {
                var item = modifications.NewObjects[i];
                var existingItem = this.getObject(item.ID);
                if (existingItem == null) {
                    this.addObject(item);
                }
                else {
                    this.mergeObjects(item, existingItem);
                }
            }
            for (var i = 0; i < modifications.CollectionAdditions.length; i++) {
                var addition = modifications.CollectionAdditions[i];
                var collectionVM = this.getObject(addition.CollectionId);
                var elementVM = this.getObject(addition.ElementId);
                if (collectionVM != null) {
                    collectionVM.Items.push(elementVM);
                }
            }
            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange = modifications.PropertyChanges[i];
                var existingItem = this.getObject(propertyChange.ObjectId);
                if (existingItem != null) {
                    var newPropertyValue = null;
                    if (propertyChange.ReferenceValueId != null) {
                        newPropertyValue = this.getObject(propertyChange.ReferenceValueId);
                    }
                    else {
                        newPropertyValue = propertyChange.NonReferenceValue;
                    }
                    var prop = $.grep(existingItem.Properties, function (e) {
                        return e.PropertyName == propertyChange.PropertyName;
                    }, false)[0];
                    prop.Value = newPropertyValue;
                }
            }
            for (var i = 0; i < modifications.CollectionRemovals.length; i++) {
                var removal = modifications.CollectionRemovals[i];
                var collectionVM = this.getObject(removal.CollectionId);
                var elementVM = this.getObject(removal.ElementId);
                if (collectionVM != null) {
                    var index = collectionVM.items.indexOf(elementVM);
                    if (index > -1) {
                        collectionVM.items.splice(index, 1);
                    }
                }
            }
        };
        IdentityMap.prototype.mergeObjects = function (newItem, existingItem) {
            for (var i = 0; i < existingItem.Properties.length; i++) {
                // NB: Don't replace the prop object, otherwise the bindings will break:
                existingItem.Properties[i].Value = newItem.Properties[i].Value;
            }
        };
        return IdentityMap;
    })();
    FresnelApp.IdentityMap = IdentityMap;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    // Used to ensure the Toolbox allows interaction with the Class nodes
    function ClassLibaryDirective() {
        return {
            link: function (scope, elem, attributes) {
                scope.$watchCollection('classHierarchy', function (newVal, oldVal) {
                    // Force the treeview to register any new nodes:
                    $(".sidebar .treeview").tree();
                    // Force the tooltips to respond:
                    $("[data-toggle='tooltip']").tooltip();
                });
            }
        };
    }
    FresnelApp.ClassLibaryDirective = ClassLibaryDirective;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var requires = ['ui.grid', 'ui.grid.autoResize', 'ui.grid.selection'];
    angular.module("fresnelApp", requires).service("appService", FresnelApp.AppService).service("explorerService", FresnelApp.ExplorerService).service("fresnelService", FresnelApp.FresnelService).controller("appController", FresnelApp.AppController).controller("toolboxController", FresnelApp.ToolboxController).controller("objectExplorerController", FresnelApp.ObjectExplorerController).controller("collectionExplorerController", FresnelApp.CollectionExplorerController).directive("classLibrary", FresnelApp.ClassLibaryDirective).directive("objectExplorer", FresnelApp.ObjectExplorerDirective).directive("aDisabled", FresnelApp.DisableAnchorDirective).config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.transformResponse.push(function (responseData) {
            convertDateStringsToDates(responseData);
            return responseData;
        });
    }]);
    // Taken from http://aboutcode.net/2013/07/27/json-date-parsing-angularjs.html
    // TODO: Refactor the Date conversion into a self-contained object:
    var regexIso8601 = /^(\d{4}|\+\d{6})(?:-(\d{2})(?:-(\d{2})(?:T(\d{2}):(\d{2}):(\d{2})\.(\d{1,})(Z|([\-+])(\d{2}):(\d{2}))?)?)?)?$/;
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
