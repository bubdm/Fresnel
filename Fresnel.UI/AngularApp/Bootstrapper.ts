module FresnelApp {

    var requires = ['blockUI',
        'inform', 'inform-exception', 'inform-http-exception', 'ngAnimate',
        'smart-table',
        'ui.bootstrap'];

    angular.module("fresnelApp", requires)
        .service("appService", FresnelApp.AppService)
        .service("explorerService", FresnelApp.ExplorerService)
        .service("fresnelService", FresnelApp.FresnelService)
        .service("requestBuilder", FresnelApp.RequestBuilder)
        .service("searchService", FresnelApp.SearchService)
        .controller("appController", FresnelApp.AppController)
        .controller("toolboxController", FresnelApp.ToolboxController)
        .controller("workbenchController", FresnelApp.WorkbenchController)
        .controller("explorerController", FresnelApp.ExplorerController)
        .controller("methodController", FresnelApp.MethodController)
        .controller("collectionExplorerController", FresnelApp.CollectionExplorerController)
        .controller("searchExplorerController", FresnelApp.SearchExplorerController)
        .controller("searchModalController", FresnelApp.SearchModalController)
        .directive("classLibrary", FresnelApp.ClassLibaryDirective)
        .directive("objectExplorer", FresnelApp.ExplorerDirective)
        .directive("aDisabled", FresnelApp.DisableAnchorDirective)
        .config(["$httpProvider", function ($httpProvider) {
        $httpProvider.defaults.transformResponse.push(function (responseData) {
            convertDateStringsToDates(responseData);
            return responseData;
        })
    }])
        .config(function (blockUIConfig) {
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