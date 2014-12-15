module FresnelApp {

    angular.module("fresnelApp", [])
        .service("appService", FresnelApp.AppService)
        .controller("appController", FresnelApp.AppController)
        .controller("toolboxController", FresnelApp.ToolboxController)
        .controller("explorerController", FresnelApp.ExplorerController)
        .directive("classLibrary", FresnelApp.ClassLibaryDirective)
    ;

}