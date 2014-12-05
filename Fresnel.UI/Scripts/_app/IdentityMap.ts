module FresnelApp {


    export class IdentityMap {

        public items: any[] = [];

        add(obj: IObjectVM) {
            this.items[obj.ID] = obj;
        }

        remove(key: string) {
            var index = this.items.indexOf(key);
            if (index > -1) {
                this.items.splice(index, 1);
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