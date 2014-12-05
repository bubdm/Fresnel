module FresnelApp {


    export class IdentityMap {

        private hash: any[] = [];
        public items: any[] = [];

        getItem(key: string) {
            var item = this.hash[key];
            return item;
        }

        addItem(obj: IObjectVM) {
            this.items.push(
                { 
                    key: obj.ID,
                    value: obj
                });

            this.hash[obj.ID] = obj;
        }

        removeItem(key: string) {
            var index = this.items.indexOf(key);
            if (index > -1) {
                this.items.splice(index, 1);
                delete this.hash[key];
            }
        }

        merge(delta: IdentityMapDelta) {

        }
    }

    export class IdentityMapDelta {

        public newItems;

        public modifiedItems;

        public deletedItems;
    }

}