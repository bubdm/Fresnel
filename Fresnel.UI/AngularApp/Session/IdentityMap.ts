﻿/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
module FresnelApp {


    export class IdentityMap {

        private objectMap: any[] = [];

        getObject(key: string) {
            var item = this.objectMap[key];
            return item;
        }

        addObject(obj: ObjectVM) {
            this.objectMap[obj.ID] = obj;

            var isCollection = obj.hasOwnProperty("IsCollection");
            if (isCollection) {
                var coll = <CollectionVM>obj;
                for (var i = 0; i < coll.Items.length; i++) {
                    var item = coll.Items[i];
                    this.objectMap[item.ID] = item;
                }
            }
        }

        remove(objID: string) {
            delete this.objectMap[objID];
        }

        merge(modifications: any) {
            if (modifications == null)
                return;

            // 1: Add new objects:
            for (var i = 0; i < modifications.NewObjects.length; i++) {
                var item: ObjectVM = modifications.NewObjects[i];
                var existingItem = this.getObject(item.ID);
                if (existingItem == null) {
                    this.addObject(item);
                }
                else {
                    // Merge the objects:
                    this.extendDeep(existingItem, item);
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
                    return e.InternalName == propertyChange.PropertyName;
                }, false)[0];

                this.extendDeep(prop.State, propertyChange.State);

                prop.State.Value = newPropertyValue;
            }

            // 3: Perform removals:
            for (var i = 0; i < modifications.CollectionRemovals.length; i++) {
                var removal = modifications.CollectionRemovals[i];
                var collectionVM = this.getObject(removal.CollectionId);
                var elementVM = this.getObject(removal.ElementId);
                if (collectionVM != null) {
                    var index = collectionVM.Items.indexOf(elementVM);
                    if (index > -1) {
                        collectionVM.Items.splice(index, 1);
                    }
                }
            }
        }

        mergeObjects(existingObj: ObjectVM, newObj: ObjectVM) {
            for (var i = 0; i < existingObj.Properties.length; i++) {
                // NB: Don't replace the prop object, otherwise the bindings will break:
                this.extendDeep(existingObj.Properties[i].State, newObj.Properties[i].State);
            }
        }

        extendDeep(destination, source) {
            for (var property in source) {
                if (source[property] && source[property].constructor &&
                    source[property].constructor === Object) {
                    destination[property] = destination[property] || {};
                    arguments.callee(destination[property], source[property]);
                } else {
                    destination[property] = source[property];
                }
            }
            return destination;
        }

        reset() {
            this.objectMap = [];
        }

    }

}