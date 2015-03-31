/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
module FresnelApp {


    export class IdentityMap {

        private objectMap: ObjectVM[] = [];

        getObject(key: string): ObjectVM {
            var item = this.objectMap[key];
            return item;
        }

        addObject(obj: ObjectVM) {
            this.objectMap[obj.ID] = obj;

            // Optimisation: By default, the Properties don't have the ObjectID set
            // So we have to do it here:
            if (obj.Properties) {
                for (var i = 0; i < obj.Properties.length; i++) {
                    var prop: PropertyVM = obj.Properties[i];
                    prop.ObjectID = obj.ID;
                }
            }

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
            this.mergeObjectTitleChanges(modifications);
            this.mergeRemovals(modifications);
            this.mergeParameterChanges(modifications);
        }

        private mergeNewObjects(modifications: ModificationsVM) {
            if (!modifications.NewObjects)
                return;

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
                var addition: CollectionElementVM = modifications.CollectionAdditions[i];
                var collection = <CollectionVM>this.getObject(addition.CollectionId);
                var element = this.getObject(addition.ElementId);
                if (collection != null) {
                    collection.Items.push(element);
                }
            }
        }

        private mergeObjectTitleChanges(modifications: ModificationsVM) {
            if (!modifications.ObjectTitleChanges)
                return;

            for (var i = 0; i < modifications.ObjectTitleChanges.length; i++) {
                var titleChange: ObjectTitleChangeVM = modifications.ObjectTitleChanges[i];

                var existingItem = this.getObject(titleChange.ObjectId);
                if (existingItem == null) {
                    continue;
                }

                existingItem.Name = titleChange.Title;
            }
        }

        private mergePropertyChanges(modifications: ModificationsVM) {
            if (!modifications.PropertyChanges)
                return;

            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange: PropertyChangeVM = modifications.PropertyChanges[i];

                var existingItem = this.getObject(propertyChange.ObjectId);
                if (existingItem == null) {
                    continue;
                }

                var prop: PropertyVM = $.grep(existingItem.Properties, function (e: PropertyVM) {
                    return e.InternalName == propertyChange.PropertyName;
                }, false)[0];

                this.extendDeep(prop.State, propertyChange.State);

                // Finally, we can set the property value:
                var newPropertyValue = null;
                if (propertyChange.State.ReferenceValueID != null) {
                    newPropertyValue = this.getObject(propertyChange.State.ReferenceValueID);
                }
                else {
                    newPropertyValue = propertyChange.State.Value;
                }

                prop.State.Value = newPropertyValue;
            }
        }

        private mergeRemovals(modifications: ModificationsVM) {
            if (!modifications.CollectionRemovals)
                return;

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

        private mergeParameterChanges(modifications: ModificationsVM) {
            if (!modifications.MethodParameterChanges)
                return;

            for (var i = 0; i < modifications.MethodParameterChanges.length; i++) {
                var parameterChange: ParameterChangeVM = modifications.MethodParameterChanges[i];

                var existingItem = this.getObject(parameterChange.ObjectId);
                if (existingItem == null) {
                    continue;
                }

                var method: MethodVM = $.grep(existingItem.Methods, function (e: MethodVM) {
                    return e.InternalName == parameterChange.MethodName;
                }, false)[0];

                var param: ParameterVM = $.grep(method.Parameters, function (e: ParameterVM) {
                    return e.InternalName == parameterChange.ParameterName;
                }, false)[0];

                this.extendDeep(param.State, parameterChange.State);

                // Finally, we can set the parameter value:
                var paramValue = null;
                if (parameterChange.State.ReferenceValueID != null) {
                    paramValue = this.getObject(parameterChange.State.ReferenceValueID);
                }
                else {
                    paramValue = parameterChange.State.Value;
                }

                param.State.Value = paramValue;
            }
        }

        mergeObjects(existingObj: ObjectVM, newObj: ObjectVM) {
            var doesExistingObjHaveProperties = (existingObj.Properties != null) && (existingObj.Properties.length > 0);
            var doesNewObjHaveProperties = (newObj.Properties != null) && (newObj.Properties.length > 0);
            var doObjectsHaveSameProperties = doesExistingObjHaveProperties && doesNewObjHaveProperties && (existingObj.Properties.length == newObj.Properties.length);

            if (!doesExistingObjHaveProperties && doesNewObjHaveProperties) {
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

            if (!doesExistingObjHaveMethods && doesNewObjHaveMethods) {
                existingObj.Methods = newObj.Methods;
            }
            else if (doObjectsHaveSameMethods) {
                for (var i = 0; i < existingObj.Methods.length; i++) {
                    this.extendDeep(existingObj.Methods[i], newObj.Methods[i]);
                }
            }

            if (!existingObj.DirtyState && newObj.DirtyState) {
                existingObj.DirtyState == newObj.DirtyState;
            }
            else {
                this.extendDeep(existingObj.DirtyState, newObj.DirtyState);
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