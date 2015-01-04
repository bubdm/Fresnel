var FresnelApp;
(function (FresnelApp) {
    var IdentityMap = (function () {
        function IdentityMap() {
            this.hash = [];
            this.items = [];
        }
        IdentityMap.prototype.getItem = function (key) {
            var item = this.hash[key];
            return item;
        };
        IdentityMap.prototype.addItem = function (obj) {
            this.removeItem(obj.ID);
            this.items.push({
                key: obj.ID,
                value: obj
            });
            this.hash[obj.ID] = obj;
            this.attachMembers(obj);
        };
        IdentityMap.prototype.attachMembers = function (obj) {
            if (obj.IsCollection) {
                for (var i = 0; i < obj.Items.length; i++) {
                    this.attachMembers(obj.Items[i]);
                }
            }
            if (obj.Properties) {
                for (var i = 0; i < obj.Properties.length; i++) {
                    var prop = obj.Properties[i];
                    obj[prop.PropertyName] = prop;
                }
            }
            if (obj.Methods) {
                for (var i = 0; i < obj.Methods.length; i++) {
                    var method = obj.Methods[i];
                    obj[method.MethodName] = method;
                }
            }
        };
        IdentityMap.prototype.removeItem = function (key) {
            var index = this.items.indexOf(key);
            if (index > -1) {
                this.items.splice(index, 1);
                delete this.hash[key];
            }
        };
        IdentityMap.prototype.merge = function (modifications) {
            for (var i = 0; i < modifications.NewObjects.length; i++) {
                var item = modifications.NewObjects[i];
                var existingItem = this.getItem(item.ID);
                if (existingItem == null) {
                    this.addItem(item);
                }
                else {
                    this.mergeObjects(item, existingItem);
                }
            }
            for (var i = 0; i < modifications.PropertyChanges.length; i++) {
                var propertyChange = modifications.PropertyChanges[i];
                var existingItem = this.getItem(propertyChange.ObjectId);
                if (existingItem != null) {
                    var newPropertyValue = null;
                    if (propertyChange.ReferenceValueId != null) {
                        newPropertyValue = this.getItem(propertyChange.ReferenceValueId);
                    }
                    else {
                        newPropertyValue = propertyChange.NonReferenceValue;
                    }
                    existingItem[propertyChange.PropertyName] = newPropertyValue;
                }
            }
            for (var i = 0; i < modifications.CollectionAdditions.length; i++) {
                var addition = modifications.CollectionAdditions[i];
                var collectionVM = this.getItem(addition.CollectionId);
                var elementVM = this.getItem(addition.ElementId);
                if (collectionVM != null) {
                    collectionVM.Items.push(elementVM);
                }
            }
            for (var i = 0; i < modifications.CollectionRemovals.length; i++) {
                var removal = modifications.CollectionRemovals[i];
                var collectionVM = this.getItem(removal.CollectionId);
                var elementVM = this.getItem(removal.ElementId);
                if (collectionVM != null) {
                    var index = collectionVM.items.indexOf(elementVM);
                    if (index > -1) {
                        collectionVM.items.splice(index, 1);
                    }
                }
            }
        };
        IdentityMap.prototype.mergeObjects = function (newItem, existingItem) {
            for (var prop in newItem) {
                if (existingItem.hasOwnProperty(prop)) {
                    existingItem[prop] = newItem[prop];
                }
            }
        };
        return IdentityMap;
    })();
    FresnelApp.IdentityMap = IdentityMap;
    var IdentityMapDelta = (function () {
        function IdentityMapDelta() {
        }
        return IdentityMapDelta;
    })();
    FresnelApp.IdentityMapDelta = IdentityMapDelta;
})(FresnelApp || (FresnelApp = {}));
//# sourceMappingURL=IdentityMap.js.map