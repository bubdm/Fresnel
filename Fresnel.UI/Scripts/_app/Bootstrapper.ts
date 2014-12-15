module FresnelApp {

    angular.module("fresnelApp", ['ngDock'])
        .service("appService", FresnelApp.AppService)
        .controller("appController", FresnelApp.AppController)
        .controller("toolboxController", FresnelApp.ToolboxController)
        .controller("explorerController", FresnelApp.ExplorerController)
    ;

}