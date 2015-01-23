module FresnelApp {

    // Used to ensure the Toolbox allows interaction with the Class nodes
    export function ExplorerDirective(
        $timeout: ng.ITimeoutService,
        $location: ng.ILocationService,
        $anchorScroll: ng.IAnchorScrollService
        ): ng.IDirective {
        return {
            link: function (scope: IExplorerControllerScope, elem: JQuery, attributes: ng.IAttributes) {
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

        }
    }
}