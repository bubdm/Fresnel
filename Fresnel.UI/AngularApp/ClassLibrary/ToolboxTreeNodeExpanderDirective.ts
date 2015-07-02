module FresnelApp {

    export function ToolboxTreeNodeExpanderDirective(): ng.IDirective {
        return {
            link: function (scope: IToolboxControllerScope, elem: JQuery, attributes: ng.IAttributes) {
                scope.$watchCollection('domainClassesHierarchy', function (newVal, oldVal) {
                    // This line is purely for VS "Find References", to remind Devs that this Directive 
                    // is associated with the collections:
                    var dummy = scope.domainClassesHierarchy;

                    // Force the treeview to register any new nodes:
                    $(".sidebar .treeview").tree();
                })
            }
        }
    }
}