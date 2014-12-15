module FresnelApp {

    // Used to ensure the Toolbox allows interaction with the Class nodes
    export function ClassLibaryDirective(): ng.IDirective {
        return {
            link: function (scope: IToolboxControllerScope, elem: JQuery, attributes: ng.IAttributes) {
                scope.$watchCollection('classHierarchy', function (newVal, oldVal) {
                    // Force the treeview to register any new nodes:
                    $(".sidebar .treeview").tree();
                })
            }

        }
    }
}