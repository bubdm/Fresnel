/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />

module FresnelApp {
    angular.module("fresnelApp", [])
        .controller("ToolboxController", FresnelApp.ToolboxController)
        .controller("ExplorerController", FresnelApp.ExplorerController);
}