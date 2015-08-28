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
            this.mergeDirtyStateChanges(modifications);
        }

        private mergeNewObjects(modifications: ModificationsVM) {
            if (!modifications.NewObjects)
                return;

            for (var i = 0; i < modifications.NewObjects.length; i++) {
                var newObj: ObjectVM = modifications.NewObjects[i];
                var existingObj = this.getObject(newObj.ID);
                if (existingObj == null) {
                    this.addObject(newObj);
                }
                else {
                    // Merge the objects:
                    this.extendDeep(existingObj, newObj);
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

                var existingObj = this.getObject(titleChange.ObjectID);
                if (existingObj == null) {
                    continue;
                }

                existingObj.Name = titleChange.Title;
            }
        }

        private mergePropertyChanges(modifications: ModificationsVM) {
            if (!modifications.PropertyChanges)
                return;

            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange: PropertyChangeVM = modifications.PropertyChanges[i];

                var existingObj = this.getObject(propertyChange.ObjectID);
                if (existingObj == null) {
                    continue;
                }

                var existingProp: PropertyVM = $.grep(existingObj.Properties, function (e: PropertyVM) {
                    return e.InternalName == propertyChange.PropertyName;
                }, false)[0];

                this.extendDeep(existingProp.State, propertyChange.State);

                // Finally, we can set the property value:
                var newPropertyValue = null;
                if (propertyChange.State.ReferenceValueID != null) {
                    newPropertyValue = this.getObject(propertyChange.State.ReferenceValueID);
                }
                else {
                    newPropertyValue = propertyChange.State.Value;
                }

                existingProp.State.Value = newPropertyValue;
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

                var existingObj = this.getObject(parameterChange.ObjectID);
                if (existingObj == null) {
                    continue;
                }

                var existingMethod: MethodVM = $.grep(existingObj.Methods, function (e: MethodVM) {
                    return e.InternalName == parameterChange.MethodName;
                }, false)[0];

                var existingParam: ParameterVM = $.grep(existingMethod.Parameters, function (e: ParameterVM) {
                    return e.InternalName == parameterChange.ParameterName;
                }, false)[0];

                this.extendDeep(existingParam.State, parameterChange.State);

                // Finally, we can set the parameter value:
                var paramValue = null;
                if (parameterChange.State.ReferenceValueID != null) {
                    paramValue = this.getObject(parameterChange.State.ReferenceValueID);
                }
                else {
                    paramValue = parameterChange.State.Value;
                }

                existingParam.State.Value = paramValue;
            }
        }

        private mergeDirtyStateChanges(modifications: ModificationsVM) {
            if (!modifications.DirtyStateChanges)
                return;

            for (var i = 0; i < modifications.DirtyStateChanges.length; i++) {
                var dirtyState = modifications.DirtyStateChanges[i];

                var existingObj = this.getObject(dirtyState.ObjectID);
                if (existingObj == null) {
                    continue;
                }

                this.extendDeep(existingObj.DirtyState, dirtyState);
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
            // We don't want to remove pinned objects:
            var itemsToRemove = $.grep(this.objectMap, function (obj: ObjectVM) {
                return !obj.IsPinned;
            });

            for (var i = 0; i < itemsToRemove.length; i++) {
                var obj = itemsToRemove[i];
                delete this.objectMap[obj.ID];
            }
        }

    }

}