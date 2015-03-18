/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
module FresnelApp {


    export class IdentityMap {

        private objectMap: any[] = [];

        getObject(key: string): ObjectVM {
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

        merge(modifications: ModificationsVM) {
            if (modifications == null)
                return;

            this.mergeNewObjects(modifications);
            this.mergePropertyChanges(modifications);
            this.mergeRemovals(modifications);
        }

        private mergeNewObjects(modifications: ModificationsVM) {
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
                var collection = <CollectionVM>this.getObject(addition.CollectionId);
                var element = this.getObject(addition.ElementId);
                if (collection != null) {
                    collection.Items.push(element);
                }
            }
        }

        private mergePropertyChanges(modifications: ModificationsVM) {
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

                var prop: PropertyVM = $.grep(existingItem.Properties, function (e: PropertyVM) {
                    return e.InternalName == propertyChange.PropertyName;
                }, false)[0];

                this.extendDeep(prop.State, propertyChange.State);

                prop.State.Value = newPropertyValue;
            }
        }

        private mergeRemovals(modifications: ModificationsVM) {
            for (var i = 0; i < modifications.CollectionRemovals.length; i++) {
                var removal = modifications.CollectionRemovals[i];
                var collection = <CollectionVM>this.getObject(removal.CollectionId);
                var element = this.getObject(removal.ElementId);
                if (collection != null) {
                    var index = collection.Items.indexOf(element);
                    if (index > -1) {
                        collection.Items.splice(index, 1);
                    }
                }
            }
        }

        mergeObjects(existingObj: ObjectVM, newObj: ObjectVM) {
            // NB: We have to be selective, otherwise the Angular bindings will break:

            var doesExistingObjHaveProperties = (existingObj.Properties != null) && (existingObj.Properties.length > 0);
            var doesNewObjHaveProperties = (newObj.Properties != null) && (newObj.Properties.length > 0);
            var doObjectsHaveSameProperties = doesExistingObjHaveProperties && doesNewObjHaveProperties && (existingObj.Properties.length == newObj.Properties.length);

            if (doesExistingObjHaveProperties) {
                // Do nothing:
            }
            else if (!doesExistingObjHaveProperties && doesNewObjHaveProperties) {
                existingObj.Properties = newObj.Properties;
            }
            else if (doObjectsHaveSameProperties) {
                for (var i = 0; i < existingObj.Properties.length; i++) {
                    this.extendDeep(existingObj.Properties[i], newObj.Properties[i]);
                }
            }

            var doesExistingObjHaveMethods = (existingObj.Methods != null) && (existingObj.Methods.length > 0);
            var doesNewObjHaveMethods = (newObj.Methods != null) && (newObj.Methods.length > 0);
            var doObjectsHaveSameMethods = doesExistingObjHaveMethods && doesNewObjHaveMethods && (existingObj.Methods.length == newObj.Methods.length);

            if (doesExistingObjHaveMethods) {
                // Do nothing:
            }
            else if (!doesExistingObjHaveMethods && doesNewObjHaveMethods) {
                existingObj.Methods = newObj.Methods;
            }
            else if (doObjectsHaveSameMethods) {
                for (var i = 0; i < existingObj.Methods.length; i++) {
                    this.extendDeep(existingObj.Methods[i], newObj.Methods[i]);
                }
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