module FresnelApp {

    export class ExplorerService {

        private explorers: any[] = [];

        addExplorer(obj: IObjectVM): Explorer {
            var explorer = new Explorer();
            explorer.__meta = obj;
            this.attachMembers(explorer);

            this.explorers[obj.ID] = explorer;
            return explorer;
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
                    explorer[prop.PropertyName] = prop;
                }
            }

            if (obj.Methods) {
                for (var i = 0; i < obj.Methods.length; i++) {
                    var method = obj.Methods[i];
                    explorer[method.MethodName] = method;
                }
            }
        }

    }

}