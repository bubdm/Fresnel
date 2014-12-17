module FresnelApp {

    var requires = ['ui.grid', 'ui.grid.autoResize', 'ui.grid.selection'];

    angular.module("fresnelApp", requires)
        .service("appService", FresnelApp.AppService)
        .controller("appController", FresnelApp.AppController)
        .controller("toolboxController", FresnelApp.ToolboxController)
        .controller("objectExplorerController", FresnelApp.ObjectExplorerController)
        .controller("collectionExplorerController", FresnelApp.CollectionExplorerController)
        .directive("classLibrary", FresnelApp.ClassLibaryDirective)
        .directive("aDisabled", FresnelApp.DisableAnchorDirective)
    ;

}