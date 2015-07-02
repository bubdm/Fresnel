module FresnelApp {

    export function ToolboxTreeNodeTooltipDirective(): ng.IDirective {
        return {
            link: function (scope: IToolboxControllerScope, elem: JQuery, attributes: ng.IAttributes) {
                scope.$watchCollection('domainClassesHierarchy', function (newVal, oldVal) {
                    // This line is purely for VS "Find References", to remind Devs that this Directive 
                    // is associated with the collections:
                    var dummy = scope.domainClassesHierarchy;

                    // Force the tooltips to respond:
                    $("[data-toggle='tooltip']").tooltip();
                })
            }
        }
    }
}