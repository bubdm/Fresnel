module FresnelApp {

    var requires = ['ui.grid', 'ui.grid.autoResize', 'ui.grid.selection', 'ui.grid.edit'];

    angular.module("fresnelApp", requires)
        .service("appService", FresnelApp.AppService)
        .service("explorerService", FresnelApp.ExplorerService)
        .service("fresnelService", FresnelApp.FresnelService)
        .controller("appController", FresnelApp.AppController)
        .controller("toolboxController", FresnelApp.ToolboxController)
        .controller("objectExplorerController", FresnelApp.ObjectExplorerController)
        .controller("collectionExplorerController", FresnelApp.CollectionExplorerController)
        .directive("classLibrary", FresnelApp.ClassLibaryDirective)
        .directive("objectExplorer", FresnelApp.ObjectExplorerDirective)
        .directive("aDisabled", FresnelApp.DisableAnchorDirective)
        .config(["$httpProvider", function ($httpProvider) {
            $httpProvider.defaults.transformResponse.push(function (responseData) {
                convertDateStringsToDates(responseData);
                return responseData;
            })
        }]);

    // Taken from http://aboutcode.net/2013/07/27/json-date-parsing-angularjs.html
    // TODO: Refactor the Date conversion into a self-contained object:

    var regexIso8601 = /^(\d{4}|\+\d{6})(?:-(\d{2})(?:-(\d{2})(?:T(\d{2}):(\d{2}):(\d{2})\.(\d{1,})(Z|([\-+])(\d{2}):(\d{2}))?)?)?)?$/;

    function convertDateStringsToDates(input) {
        // Ignore things that aren't objects.
        if (typeof input !== "object") return input;

        for (var key in input) {
            if (!input.hasOwnProperty(key)) continue;

            var value = input[key];
            var match;
            // Check for string properties which look like dates.
            if (typeof value === "string" && (match = value.match(regexIso8601))) {
                var milliseconds = Date.parse(match[0])
                if (!isNaN(milliseconds)) {
                    input[key] = new Date(milliseconds);
                }
            } else if (typeof value === "object") {
                // Recurse into object
                convertDateStringsToDates(value);
            }
        }
    }
}