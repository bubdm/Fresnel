module FresnelApp {


    export class IdentityMap {

        public items: any[] = [];

        add(key, value) {
            this.items.push({
                key: key,
                value: value
            });
        }

        remove(key) {
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