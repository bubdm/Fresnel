var FresnelApp;
(function (FresnelApp) {
    var CollectionExplorerController = (function () {
        function CollectionExplorerController($scope, $http, appService) {
            $scope.gridColumns = [];
            var collection = $scope.obj;
            for (var i = 0; i < collection.ColumnHeaders.length; i++) {
                var newColumn = {
                    name: collection.ColumnHeaders[i].Name,
                    field: collection.ColumnHeaders[i].PropertyName + ".Value"
                };
                $scope.gridColumns[i] = newColumn;
            }
            $scope.gridOptions = {
                enableSorting: true,
                columnDefs: $scope.gridColumns,
                data: collection.Items
            };
        }
        CollectionExplorerController.$inject = ['$scope', '$http', 'appService'];
        return CollectionExplorerController;
    })();
    FresnelApp.CollectionExplorerController = CollectionExplorerController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var ObjectExplorerController = (function () {
        function ObjectExplorerController($scope, $http, appService) {
            $scope.visibleExplorers = [];
            $scope.$on('objectCreated', function (event, obj) {
                $scope.visibleExplorers.push(obj);
            });
            $scope.invoke = function (method) {
                var uri = "api/Explorer/InvokeMethod";
                $http.post(uri, method).success(function (data, status) {
                    appService.identityMap.merge(data.Modifications);
                });
            };
            $scope.setProperty = function (prop) {
                var uri = "api/Explorer/SetProperty";
                var request = {
                    ObjectId: prop.ObjectID,
                    PropertyName: prop.PropertyName,
                    NonReferenceValue: prop.Value
                };
                $http.post(uri, request).success(function (data, status) {
                    appService.identityMap.merge(data.Modifications);
                    appService.mergeMessages(data.Messages);
                });
            };
            $scope.minimise = function (obj) {
                obj.IsMaximised = false;
            };
            $scope.maximise = function (obj) {
                obj.IsMaximised = true;
            };
            $scope.close = function (obj) {
                // TODO: Check for dirty status
                var index = $scope.visibleExplorers.indexOf(obj);
                if (index > -1) {
                    $scope.visibleExplorers.splice(index, 1);
                }
            };
            $scope.openNewExplorer = function (prop) {
                var uri = "api/Explorer/GetObjectProperty";
                $http.post(uri, prop).success(function (data, status) {
                    var obj = data.ReturnValue;
                    if (obj) {
                        appService.identityMap.addItem(obj);
                        // TODO: Insert the object just after it's parent?
                        obj.OuterProperty = prop;
                        $scope.visibleExplorers.push(obj);
                    }
                });
            };
        }
        ObjectExplorerController.$inject = ['$scope', '$http', 'appService'];
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
    var AppController = (function () {
        function AppController($scope, $http, appService) {
            appService.identityMap = new FresnelApp.IdentityMap();
            $scope.identityMap = appService.identityMap;
            $scope.loadSession = function () {
                var uri = "api/Session/GetSession";
                $http.get(uri).success(function (data, status) {
                    $scope.session = data;
                    appService.session = $scope.session;
                });
            };
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadSession();
            });
        }
        AppController.$inject = ['$scope', '$http', 'appService'];
        return AppController;
    })();
    FresnelApp.AppController = AppController;
})(FresnelApp || (FresnelApp = {}));
var FresnelApp;
(function (FresnelApp) {
    var AppService = (function () {
        function AppService() {
        }
        AppService.prototype.mergeMessages = function (messageSet) {
            this.mergeMessageArray(messageSet.Infos, this.session.Messages.Infos);
            this.mergeMessageArray(messageSet.Warnings, this.session.Messages.Warnings);
            this.mergeMessageArray(messageSet.Errors, this.session.Messages.Errors);
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
        function ToolboxController($rootScope, $scope, $http, appService) {
            $scope.loadClassHierarchy = function () {
                var _this = this;
                var uri = "api/Toolbox/GetClassHierarchy";
                $http.get(uri).success(function (data, status) { return _this.classHierarchy = data; });
            };
            $scope.create = function (fullyQualifiedName) {
                var uri = "api/Toolbox/Create";
                var arg = "=" + fullyQualifiedName;
                var config = {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8' }
                };
                $http.post(uri, arg, config).success(function (data, status) {
                    var newObject = data.NewObject;
                    appService.identityMap.addItem(newObject);
                    $rootScope.$broadcast("objectCreated", newObject);
                });
            };
            // This will run when the page loads:
            angular.element(document).ready(function () {
                $scope.loadClassHierarchy();
            });
        }
        ToolboxController.$inject = ['$rootScope', '$scope', '$http', 'appService'];
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
var FresnelApp;
(function (FresnelApp) {
    var IdentityMap = (function () {
        function IdentityMap() {
            this.hash = [];
            this.items = [];
        }
        IdentityMap.prototype.getItem = function (key) {
            var item = this.hash[key];
            return item;
        };
        IdentityMap.prototype.addItem = function (obj) {
            this.removeItem(obj.ID);
            this.items.push({
                key: obj.ID,
                value: obj
            });
            this.hash[obj.ID] = obj;
            this.attachMembers(obj);
        };
        IdentityMap.prototype.attachMembers = function (obj) {
            if (obj.IsCollection) {
                for (var i = 0; i < obj.Items.length; i++) {
                    this.attachMembers(obj.Items[i]);
                }
            }
            if (obj.Properties) {
                for (var i = 0; i < obj.Properties.length; i++) {
                    var prop = obj.Properties[i];
                    obj[prop.PropertyName] = prop;
                }
            }
            if (obj.Methods) {
                for (var i = 0; i < obj.Methods.length; i++) {
                    var method = obj.Methods[i];
                    obj[method.MethodName] = method;
                }
            }
        };
        IdentityMap.prototype.removeItem = function (key) {
            var index = this.items.indexOf(key);
            if (index > -1) {
                this.items.splice(index, 1);
                delete this.hash[key];
            }
        };
        IdentityMap.prototype.merge = function (modifications) {
            if (modifications == null)
                return;
            for (var i = 0; i < modifications.NewObjects.length; i++) {
                var item = modifications.NewObjects[i];
                var existingItem = this.getItem(item.ID);
                if (existingItem == null) {
                    this.addItem(item);
                }
                else {
                    this.mergeObjects(item, existingItem);
                }
            }
            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange = modifications.PropertyChanges[i];
                var existingItem = this.getItem(propertyChange.ObjectId);
                if (existingItem != null) {
                    var newPropertyValue = null;
                    if (propertyChange.ReferenceValueId != null) {
                        newPropertyValue = this.getItem(propertyChange.ReferenceValueId);
                    }
                    else {
                        newPropertyValue = propertyChange.NonReferenceValue;
                    }
                    existingItem[propertyChange.PropertyName].Value = newPropertyValue;
                }
            }
            for (var i = 0; i < modifications.CollectionAdditions.length; i++) {
                var addition = modifications.CollectionAdditions[i];
                var collectionVM = this.getItem(addition.CollectionId);
                var elementVM = this.getItem(addition.ElementId);
                if (collectionVM != null) {
                    collectionVM.Items.push(elementVM);
                }
            }
            for (var i = 0; i < modifications.CollectionRemovals.length; i++) {
                var removal = modifications.CollectionRemovals[i];
                var collectionVM = this.getItem(removal.CollectionId);
                var elementVM = this.getItem(removal.ElementId);
                if (collectionVM != null) {
                    var index = collectionVM.items.indexOf(elementVM);
                    if (index > -1) {
                        collectionVM.items.splice(index, 1);
                    }
                }
            }
        };
        IdentityMap.prototype.mergeObjects = function (newItem, existingItem) {
            for (var prop in newItem) {
                if (existingItem.hasOwnProperty(prop)) {
                    existingItem[prop] = newItem[prop];
                }
            }
        };
        return IdentityMap;
    })();
    FresnelApp.IdentityMap = IdentityMap;
    var IdentityMapDelta = (function () {
        function IdentityMapDelta() {
        }
        return IdentityMapDelta;
    })();
    FresnelApp.IdentityMapDelta = IdentityMapDelta;
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
    angular.module("fresnelApp", requires).service("appService", FresnelApp.AppService).controller("appController", FresnelApp.AppController).controller("toolboxController", FresnelApp.ToolboxController).controller("objectExplorerController", FresnelApp.ObjectExplorerController).controller("collectionExplorerController", FresnelApp.CollectionExplorerController).directive("classLibrary", FresnelApp.ClassLibaryDirective).directive("objectExplorer", FresnelApp.ObjectExplorerDirective).directive("aDisabled", FresnelApp.DisableAnchorDirective).config(["$httpProvider", function ($httpProvider) {
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
