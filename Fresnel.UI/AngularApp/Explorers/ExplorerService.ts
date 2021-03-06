module FresnelApp {

    export class ExplorerService {

        private templateCache: ng.ITemplateCacheService;
        private explorers: Explorer[] = [];

        static $inject = ['$templateCache'];

        constructor(
            $templateCache: ng.ITemplateCacheService
            ) {
            this.templateCache = $templateCache;
        }

        addExplorer(obj: ObjectVM): Explorer {
            var explorer: Explorer = {
                __meta: obj,
                ParentRow: null,
                ParentExplorer: null,
                IsMaximised: true,
                CustomTemplateUrl: null,
            };
            this.CheckForCustomTemplate(explorer);

            this.attachMembers(explorer);

            this.explorers[obj.ID] = explorer;
            return explorer;
        }

        CheckForCustomTemplate(explorer: Explorer) {
            var templateUrl = "/Customisations/" + explorer.__meta.Type + ".html";

            // We're using XMLHttpRequest so that failures don't propogate to angular-inform:
            var request = new XMLHttpRequest();
            request.open('HEAD', templateUrl, false);
            request.send();
            if (request.status == 200) {
                explorer.CustomTemplateUrl = templateUrl;
            }
            else {
                // This ensures that newer templates are picked up next time:
                this.templateCache.remove(templateUrl);
            }
        }

        getExplorer(objID: string): Explorer {
            var result = this.explorers[objID];
            return result;
        }

        remove(explorer: Explorer) {
            var objID = explorer.__meta.ID;
            delete this.explorers[objID];
        }

        attachMembers(explorer: Explorer) {
            var obj = explorer.__meta;

            if (obj.Properties) {
                for (var i = 0; i < obj.Properties.length; i++) {
                    var prop = obj.Properties[i];
                    explorer[prop.InternalName] = prop;
                }
            }

            if (obj.Methods) {
                for (var i = 0; i < obj.Methods.length; i++) {
                    var method = obj.Methods[i];
                    explorer[method.InternalName] = method;
                }
            }
        }

    }

}