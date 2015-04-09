module FresnelApp {

    export class RequestBuilder {

        buildCreateObjectRequest(obj: ObjectVM, fullClassTypeName: string) {
            var request: CreateRequest = {
                ParentObjectID: obj != null ? obj.ID : null,
                ClassTypeName: fullClassTypeName
            };

            return request;
        }

        buildSetParameterRequest(obj: ObjectVM, method: MethodVM, param: ParameterVM) {
            var request: SetParameterRequest = {
                ObjectID: obj.ID,
                MethodName: method.InternalName,
                ParameterName: param.InternalName,
                NonReferenceValue: param.State.Value,
                ReferenceValueId: param.State.ReferenceValueID,
            };

            return request;
        }

        buildMethodInvokeRequest(method: MethodVM) {
            var request: InvokeMethodRequest = {
                ObjectID: method.ObjectID,
                MethodName: method.InternalName,
                Parameters: []
            };

            for (var i = 0; i < method.Parameters.length; i++) {
                var param = method.Parameters[i];
                var requestParam: any = {
                    InternalName: param.InternalName,
                    State: {
                        Value: param.State.Value,
                        ReferenceValueID: param.State.ReferenceValueID
                    }
                }
                request.Parameters.push(requestParam);
            }

            return request;
        }

        buildCreateAndAssociateRequest(prop: PropertyVM, classTypeName: string) {
            var request: CreateAndSetPropertyRequest = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                ClassTypeName: classTypeName
            };

            return request;
        }

        buildSetPropertyRequest(prop: PropertyVM) {
            var request: SetPropertyRequest = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                NonReferenceValue: prop.State.Value,
                ReferenceValueId: prop.State.ReferenceValueID
            };

            return request;
        }

        buildGetPropertyRequest(prop: PropertyVM) {
            var request: GetPropertyRequest = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName
            };

            return request;
        }

        buildGetObjectRequest(obj: ObjectVM) {
            var request: GetObjectRequest = {
                ObjectID: obj.ID
            };

            return request;
        }

        buildSaveChangesRequest(obj: ObjectVM) {
            var request: SaveChangesRequest = {
                ObjectID: obj.ID
            };

            return request;
        }

        buildSearchObjectsRequest(fullyQualifiedName: string) {
            var request: SearchObjectsRequest = {
                SearchType: fullyQualifiedName,
                SearchFilters: null,
                OrderBy: null,
                IsDescendingOrder: false,
                PageNumber: 1,
                PageSize: 100
            };

            return request;
        }

        buildSearchPropertyRequest(prop: PropertyVM) {
            var request: SearchPropertyRequest = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                SearchFilters: null,
                OrderBy: null,
                IsDescendingOrder: false,
                PageNumber: 1,
                PageSize: 100
            };

            return request;
        }

        buildSearchParameterRequest(method: MethodVM, param: ParameterVM) {
            var request: SearchParameterRequest = {
                ObjectID: method.ObjectID,
                MethodName: method.InternalName,
                ParameterName: param.InternalName,
                SearchFilters: null,
                OrderBy: null,
                IsDescendingOrder: false,
                PageNumber: 1,
                PageSize: 100
            };

            return request;
        }

        buildAddItemsRequest(collectionProp: PropertyVM, itemsToAdd: ObjectVM[]) {
            var elementIDs = itemsToAdd.map(function (o) { return o.ID });

            var request: CollectionAddRequest = {
                ParentObjectID: collectionProp.ObjectID,
                CollectionPropertyName: collectionProp.InternalName,
                ElementIDs: elementIDs,
            };

            return request;
        }
    }

}