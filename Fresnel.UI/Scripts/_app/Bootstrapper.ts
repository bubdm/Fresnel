module FresnelApp {
    angular.module("fresnelApp", ["pageslide-directive"])
        .controller("toolboxController", FresnelApp.ToolboxController)
        .controller("explorerController", FresnelApp.ExplorerController);
}