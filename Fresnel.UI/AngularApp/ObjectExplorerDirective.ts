module FresnelApp {

    // Used to ensure the Toolbox allows interaction with the Class nodes
    export function ObjectExplorerDirective(): ng.IDirective {
        return {
            link: function (scope: IObjectExplorerControllerScope, elem: JQuery, attributes: ng.IAttributes) {
                scope.$watchCollection('visibleExplorers', function (newVal, oldVal) {

                    //bootstrap WYSIHTML5 - text editor
                    $(".textarea").wysihtml5();
                })
            }

        }
    }
}