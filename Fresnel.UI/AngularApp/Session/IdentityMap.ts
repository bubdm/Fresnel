/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
module FresnelApp {


    export class IdentityMap {

        private objectMap: any[] = [];

        getObject(key: string) {
            var item = this.objectMap[key];
            return item;
        }

        addObject(obj: IObjectVM) {
            this.remove(obj.ID);
            this.objectMap[obj.ID] = obj;
        }

        remove(objID: string) {
            delete this.objectMap[objID];
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
                    // Merge the objects:
                    angular.extend(existingItem, item);
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
                if (existingItem == null) {
                    continue;
                }

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

                angular.extend(prop.State, propertyChange.State);

                prop.State.Value = newPropertyValue;
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

        //mergeObjects(newObj: IObjectVM, existingObj: IObjectVM) {
        //    for (var i = 0; i < existingObj.Properties.length; i++) {
        //        // NB: Don't replace the prop object, otherwise the bindings will break:
        //        existingObj.Properties[i].Value = newObj.Properties[i].Value;
        //    }
        //}

    }

}