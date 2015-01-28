/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />
var FresnelApp;
(function (FresnelApp) {
    var MethodController = (function () {
        function MethodController($rootScope, $scope, fresnelService, appService, explorerService, requestBuilder, explorer, method) {
            $scope.explorer = explorer;
            $scope.method = method;
            method.ParametersSetByUser = [];
            $scope.invoke = function (method) {
                var request = requestBuilder.buildMethodInvokeRequest(method);
                var promise = fresnelService.invokeMethod(request);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    method.Error = result.Passed ? "" : result.Messages[0].Text;
                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                    if (result.ResultObject) {
                        $rootScope.$broadcast("openNewExplorer", result.ResultObject);
                    }
                });
            };
            $scope.setProperty = function (param) {
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
            };
            $scope.setBitwiseEnumProperty = function (param, enumValue) {
                param.State.Value = param.State.Value ^ enumValue;
                $scope.setProperty(param);
            };
            $scope.cancel = function () {
                //modalInstance.dismiss();
            };
        }
        MethodController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'appService',
            'explorerService',
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
            $scope.visibleExplorers = [];
            $scope.$on('openNewExplorer', function (event, obj) {
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
                var explorer = explorerService.getExplorer(obj.ID);
                if (explorer == null) {
                    explorer = explorerService.addExplorer(obj);
                    $scope.visibleExplorers.push(explorer);
                }
            });
            $scope.$on('closeExplorer', function (event, explorer) {
                var index = $scope.visibleExplorers.indexOf(explorer);
                if (index > -1) {
                    $scope.visibleExplorers.splice(index, 1);
                    explorerService.remove(explorer);
                    if ($scope.visibleExplorers.length == 0) {
                        var promise = fresnelService.cleanupSession();
                        promise.then(function (promiseResult) {
                            var result = promiseResult.data;
                            appService.identityMap.reset();
                            $rootScope.$broadcast("messagesReceived", result.Messages);
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
            var explorer = new FresnelApp.Explorer();
            explorer.__meta = obj;
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
        function CollectionExplorerController($rootScope, $scope, fresnelService, appService) {
            var collection = $scope.explorer.__meta;
            // This allows Smart-Table to handle the st-safe-src properly:
            collection.DisplayItems = [].concat(collection.Items);
            $scope.addNewItem = function (itemType) {
                var request = {
                    CollectionID: collection.ID,
                    ElementTypeName: itemType,
                    ElementID: null
                };
                var promise = fresnelService.addNewItemToCollection(request);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                    // This will cause the new object to appear in a new Explorer:
                    //$rootScope.$broadcast("openNewExplorer", newObject);             
                });
            };
            $scope.addExistingItem = function (obj) {
                var request = {
                    CollectionID: collection.ID,
                    ElementTypeName: null,
                    ElementID: obj.ID
                };
                var promise = fresnelService.addItemToCollection(request);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                });
            };
            $scope.removeItem = function (obj) {
                var request = {
                    CollectionID: collection.ID,
                    ElementTypeName: null,
                    ElementID: obj.ID
                };
                var promise = fresnelService.removeItemFromCollection(request);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                });
            };
        }
        CollectionExplorerController.prototype.getEditorTemplate = function (prop) {
            if (prop.Info == null)
                return;
            switch (prop.Info.Name) {
                case "boolean":
                    switch (prop.Info.PreferredControl) {
                        default:
                            return '/Templates/Editors/booleanRadioEditor.html';
                    }
                case "datetime":
                    switch (prop.Info.PreferredControl) {
                        case "Date":
                            return '/Templates/Editors/dateEditor.html';
                        case "Time":
                            return '/Templates/Editors/timeEditor.html';
                        default:
                            return '/Templates/Editors/dateTimeEditor.html';
                    }
                case "enum":
                    switch (prop.Info.PreferredControl) {
                        case "Checkbox":
                            return '/Templates/Editors/enumCheckboxEditor.html';
                        case "Radio":
                            return '/Templates/Editors/enumRadioEditor.html';
                        default:
                            return '/Templates/Editors/enumSelectEditor.html';
                    }
                case "string":
                    switch (prop.Info.PreferredControl) {
                        case "Password":
                            return '/Templates/Editors/passwordEditor.html';
                        case "TextArea":
                            return '/Templates/Editors/textAreaEditor.html';
                        case "RichTextArea":
                            return '/Templates/Editors/richTextEditor.html';
                        default:
                            return '/Templates/Editors/stringEditor.html';
                    }
                default:
                    return '';
            }
        };
        CollectionExplorerController.$inject = ['$rootScope', '$scope', 'fresnelService', 'appService'];
        return CollectionExplorerController;
    })();
    FresnelApp.CollectionExplorerController = CollectionExplorerController;
})(FresnelApp || (FresnelApp = {}));
/// <reference path="../../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />
var FresnelApp;
(function (FresnelApp) {
    var ExplorerController = (function () {
        function ExplorerController($rootScope, $scope, fresnelService, requestBuilder, appService, explorerService, $modal) {
            $scope.invoke = function (method) {
                if (method.Parameters.length == 0) {
                    var request = requestBuilder.buildMethodInvokeRequest(method);
                    var promise = fresnelService.invokeMethod(request);
                    promise.then(function (promiseResult) {
                        var result = promiseResult.data;
                        method.Error = result.Passed ? "" : result.Messages[0].Text;
                        appService.identityMap.merge(result.Modifications);
                        $rootScope.$broadcast("messagesReceived", result.Messages);
                        if (result.ResultObject) {
                            $rootScope.$broadcast("openNewExplorer", result.ResultObject);
                        }
                    });
                }
                else {
                    var options = {
                        templateUrl: '/Templates/methodDialog.html',
                        controller: 'methodController',
                        backdrop: 'static',
                        resolve: {
                            // These objects will be injected into the MethodController's ctor:
                            explorer: function () {
                                return $scope.explorer;
                            },
                            method: function () {
                                return method;
                            }
                        }
                    };
                    var modal = $modal.open(options);
                    $rootScope.$broadcast("modalOpened", modal);
                    modal.result.finally(function () {
                        $rootScope.$broadcast("modalClosed", modal);
                    });
                }
            };
            $scope.setProperty = function (prop) {
                var request = requestBuilder.buildSetPropertyRequest(prop);
                var promise = fresnelService.setProperty(request);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    prop.Error = result.Passed ? "" : result.Messages[0].Text;
                    appService.identityMap.merge(result.Modifications);
                    $rootScope.$broadcast("messagesReceived", result.Messages);
                });
            };
            $scope.setBitwiseEnumProperty = function (prop, enumValue) {
                prop.State.Value = prop.State.Value ^ enumValue;
                $scope.setProperty(prop);
            };
            $scope.refresh = function (explorer) {
                var request = requestBuilder.buildGetObjectRequest(explorer.__meta);
                var promise = fresnelService.getObject(request);
                promise.then(function (promiseResult) {
                    var obj = promiseResult.data.ReturnValue;
                    var existingObj = appService.identityMap.getObject(obj.ID);
                    appService.identityMap.mergeObjects(existingObj, obj);
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
                $rootScope.$broadcast("closeExplorer", explorer);
            };
            $scope.openNewExplorer = function (obj) {
                $rootScope.$broadcast("openNewExplorer", obj);
            };
            $scope.openNewExplorerForProperty = function (prop) {
                var request = requestBuilder.buildGetPropertyRequest(prop);
                var promise = fresnelService.getProperty(request);
                promise.then(function (promiseResult) {
                    var result = promiseResult.data;
                    var obj = result.ReturnValue;
                    if (obj) {
                        var existingObj = appService.identityMap.getObject(obj.ID);
                        if (existingObj == null) {
                            appService.identityMap.addObject(obj);
                        }
                        else {
                            // Re-use the existing object, so that any bindings aren't lost:
                            obj = existingObj;
                        }
                        obj.OuterProperty = prop;
                        $rootScope.$broadcast("openNewExplorer", obj);
                    }
                });
            };
        }
        ExplorerController.$inject = [
            '$rootScope',
            '$scope',
            'fresnelService',
            'requestBuilder',
            'appService',
            'explorerService',
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
            link: function (scope, elem, attributes) {
                // Scroll to the new panel:
                // We're using a delay so that the element is rendered before we inspect it:
                // See http://stackoverflow.com/a/20156250/80369
                $timeout(function () {
                    var explorer = scope.explorer;
                    var elementID = "explorer_" + explorer.__meta.ID;
                    $location.hash(elementID);
                    $anchorScroll();
                }, 0);
                //scope.$watchCollection('visibleExplorers', function (newVal, oldVal) {
                //    ////bootstrap WYSIHTML5 - text editor
                //    //$(".textarea").wysihtml5();
                //})
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
        RequestBuilder.prototype.buildMethodInvokeRequest = function (method) {
            var request = {
                ObjectID: method.ObjectID,
                MethodName: method.InternalName,
                Parameters: []
            };
            for (var i = 0; i < method.Parameters.length; i++) {
                var param = method.Parameters[i];
                var requestParam = {
                    InternalName: param.InternalName,
                    State: {
                        Value: param.State.Value,
                        ReferenceValueID: param.State.ReferenceValueID
                    }
                };
                request.Parameters.push(requestParam);
            }
            return request;
        };
        RequestBuilder.prototype.buildSetPropertyRequest = function (prop) {
            var request = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                NonReferenceValue: prop.State.Value,
                ReferenceValueId: null
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
        return RequestBuilder;
    })();
    FresnelApp.RequestBuilder = RequestBuilder;
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
        FresnelService.prototype.getObject = function (request) {
            var uri = "api/Explorer/GetObject";
            return this.http.post(uri, request);
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
        FresnelService.prototype.addNewItemToCollection = function (request) {
            var uri = "api/Explorer/AddItemToCollection";
            return this.http.post(uri, request);
        };
        FresnelService.prototype.addItemToCollection = function (request) {
            var uri = "api/Explorer/AddItemToCollection";
            return this.http.post(uri, request);
        };
        FresnelService.prototype.removeItemFromCollection = function (request) {
            var uri = "api/Explorer/RemoveItemFromCollection";
            return this.http.post(uri, request);
        };
        FresnelService.prototype.cleanupSession = function () {
            var uri = "api/Session/CleanUp";
            return this.http.get(uri);
        };
        FresnelService.$inject = ['$http'];
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
            $scope.$on('messagesReceived', function (event, messages) {
                appService.mergeMessages(messages, $scope.session);
                for (var i = 0; i < messages.length; i++) {
                    var message = messages[i];
                    var messageType = message.IsSuccess ? 'success' : message.IsInfo ? 'info' : message.IsWarning ? 'warning' : message.IsError ? 'danger' : 'default';
                    // Don't let the message automatically fade away:
                    var messageTtl = message.RequiresAcknowledgement ? 0 : 5000;
                    inform.add(message.Text, { ttl: messageTtl, type: messageType });
                }
            });
            $scope.$on('modalOpened', function (event) {
                $scope.IsModalVisible = true;
            });
            $scope.$on('modalClosed', function (event) {
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
                    $rootScope.$broadcast("openNewExplorer", newObject);
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
            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange = modifications.PropertyChanges[i];
                var existingItem = this.getObject(propertyChange.ObjectId);
                if (existingItem == null) {
                    continue;
                }
                var newPropertyValue = null;
                if (propertyChange.ReferenceValueId != null) {
                    newPropertyValue = this.getObject(propertyChange.ReferenceValueId);
                }
                else {
                    newPropertyValue = propertyChange.NonReferenceValue;
                }
                var prop = $.grep(existingItem.Properties, function (e) {
                    return e.InternalName == propertyChange.PropertyName;
                }, false)[0];
                this.extendDeep(prop.State, propertyChange.State);
                prop.State.Value = newPropertyValue;
            }
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
        IdentityMap.prototype.mergeObjects = function (existingObj, newObj) {
            for (var i = 0; i < existingObj.Properties.length; i++) {
                // NB: Don't replace the prop object, otherwise the bindings will break:
                this.extendDeep(existingObj.Properties[i].State, newObj.Properties[i].State);
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
    var requires = ['blockUI', 'inform', 'inform-exception', 'inform-http-exception', 'ngAnimate', 'smart-table', 'ui.bootstrap'];
    angular.module("fresnelApp", requires).service("appService", FresnelApp.AppService).service("explorerService", FresnelApp.ExplorerService).service("fresnelService", FresnelApp.FresnelService).service("requestBuilder", FresnelApp.RequestBuilder).controller("appController", FresnelApp.AppController).controller("toolboxController", FresnelApp.ToolboxController).controller("workbenchController", FresnelApp.WorkbenchController).controller("explorerController", FresnelApp.ExplorerController).controller("methodController", FresnelApp.MethodController).controller("collectionExplorerController", FresnelApp.CollectionExplorerController).directive("classLibrary", FresnelApp.ClassLibaryDirective).directive("objectExplorer", FresnelApp.ExplorerDirective).directive("aDisabled", FresnelApp.DisableAnchorDirective).config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.transformResponse.push(function (responseData) {
            convertDateStringsToDates(responseData);
            return responseData;
        });
    }]).config(function (blockUIConfig) {
        blockUIConfig.message = 'Please wait...';
        blockUIConfig.delay = 250;
        blockUIConfig.resetOnException = true;
    });
    // See http://aboutcode.net/2013/07/27/json-date-parsing-angularjs.html
    // and http://stackoverflow.com/a/3143231/80369
    // TODO: Refactor the Date conversion into a self-contained object:
    var regexIso8601 = /(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z))|(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d([+-][0-2]\d:[0-5]\d|Z))/;
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
