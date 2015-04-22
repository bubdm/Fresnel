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

                    var bodyElem = $("body");
                    var workbenchElem = $("#workbench");
                    var explorerElem = $("#explorer_" + explorer.__meta.ID);

                    // Handle X positioning:
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

                    // Handle Y positioning:
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

        }
    }
}