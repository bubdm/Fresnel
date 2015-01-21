module FresnelApp {

    export function GridsterAutoRowHeightDirective($timeout: ng.ITimeoutService): ng.IDirective {

        return {
            link: function (scope: IObjectExplorerControllerScope, elem: JQuery, attributes: ng.IAttributes) {
                // We're using a delay so that the element is rendered before we inspect it:
                // See http://stackoverflow.com/a/20156250/80369
                $timeout(function () {
                    var explorer = scope.explorer;

                    var id = "explorer_" + explorer.__meta.ID;
                    var content = angular.element(document.getElementById(id));

                    if (explorer.__meta.IsCollection) {
                        explorer.ColWidth = 24;
                        explorer.RowHeight = 9;
                    }
                    else {
                        // Hand tuned, based on gridsterOptions.columns:
                        var rowHeightPx = 68;
                        explorer.RowHeight = Math.round(content.height() / rowHeightPx) + 1;
                    }
                }, 0);
            }

        }
    }
}