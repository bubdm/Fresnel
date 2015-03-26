module FresnelApp {

    // Used to ensure the Toolbox allows interaction with the Class nodes
    export function ExplorerDirective(
        $timeout: ng.ITimeoutService,
        $location: ng.ILocationService,
        $anchorScroll: ng.IAnchorScrollService
        ): ng.IDirective {
        return {
            // Scroll to the new panel:
            link: function (scope: IExplorerScope, elem: JQuery, attributes: ng.IAttributes) {

                // We're using a delay so that the element is rendered before we inspect it.
                // The delay must be longer than the animation used to render the element (see animations.css):
                var delay = 501;

                // See http://stackoverflow.com/a/20156250/80369
                $timeout(function () {
                    var explorer = scope.explorer;

                    // If the panel fits in the view, it's right side should be completely visible:
                    var rightAnchorID = "explorerRightAnchor_" + explorer.__meta.ID;
                    $location.hash(rightAnchorID);
                    $anchorScroll();

                    // If the panel is wider than the container, make sure it's left side is visible:
                    var elementID = "explorer_" + explorer.__meta.ID;
                    $location.hash(elementID);
                    $anchorScroll();

                }, 1000);
            }

        }
    }
}