/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
module FresnelApp {


    export class IdentityMap {

        private objects: any[] = [];
        public explorers: any[] = [];

        getObject(key: string) {
            var item = this.objects[key];
            return item;
        }

        addObject(obj: IObjectVM) {
            // We're wrapping the Domain Object in an Explorer:
            var newExplorer = this.createExplorer(obj);
            this.remove(obj.ID);

            this.objects[obj.ID] = obj;

            this.explorers[obj.ID] = newExplorer;
            this.attachMembers(newExplorer);

            if (obj.IsCollection) {
                for (var i = 0; i < obj.Items.length; i++) {
                    var item = obj.Items[i];
                    var itemExplorer = this.getExplorer(item.ID);
                    if (itemExplorer == null) {
                        itemExplorer = this.createExplorer(item);
                    }
                    this.attachMembers(itemExplorer);
                }
            }
        }

        createExplorer(obj: IObjectVM) : Explorer {
            var explorer = new Explorer();
            explorer.__meta = obj;
            return explorer;
        }

        getExplorer(key: string) {
            var result = this.explorers[key];
            return result;
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

        remove(key: string) {
            var index = this.explorers.indexOf(key);
            if (index > -1) {
                this.explorers.splice(index, 1);
            }

            index = this.objects.indexOf(key);
            if (index > -1) {
                this.objects.splice(index, 1);
            }
        }

        merge(modifications: any) {
            if (modifications == null)
                return;

            // 1: Add new objects:
            for (var i = 0; i < modifications.NewObjects.length; i++) {
                var item: IObjectVM = modifications.NewObjects[i];
                var existingItem = this.getObject(item.ID);
                if (existingItem == null) {
                    this.addObject(item);
                }
                else {
                    this.mergeObjects(item, existingItem);
                }
            }

            for (var i = 0; i < modifications.CollectionAdditions.length; i++) {
                var addition = modifications.CollectionAdditions[i];
                var collectionVM = this.getObject(addition.CollectionId);
                var elementVM = this.getObject(addition.ElementId);
                if (collectionVM != null) {
                    collectionVM.Items.push(elementVM);
                }
            }

            // 2: Apply modifications:
            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange = modifications.PropertyChanges[i];

                var existingItem = this.getObject(propertyChange.ObjectId);
                if (existingItem != null) {
                    var newPropertyValue = null;

                    if (propertyChange.ReferenceValueId != null) {
                        newPropertyValue = this.getObject(propertyChange.ReferenceValueId);
                    }
                    else {
                        newPropertyValue = propertyChange.NonReferenceValue;
                    }

                    var prop: any = $.grep(existingItem.Properties, function (e: any) {
                        return e.PropertyName == propertyChange.PropertyName;
                    }, false)[0];
                    prop.Value = newPropertyValue;
                }
            }

            // 3: Perform removals:
            for (var i = 0; i < modifications.CollectionRemovals.length; i++) {
                var removal = modifications.CollectionRemovals[i];
                var collectionVM = this.getObject(removal.CollectionId);
                var elementVM = this.getObject(removal.ElementId);
                if (collectionVM != null) {
                    var index = collectionVM.items.indexOf(elementVM);
                    if (index > -1) {
                        collectionVM.items.splice(index, 1);
                    }
                }
            }
        }

        mergeObjects(newItem: IObjectVM, existingItem: IObjectVM) {
            for (var prop in newItem) {
                if (existingItem.hasOwnProperty(prop)) {
                    existingItem[prop] = newItem[prop];
                }
            }
        }

    }

}