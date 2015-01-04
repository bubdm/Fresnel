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
//# sourceMappingURL=ObjectExplorerDirective.js.map