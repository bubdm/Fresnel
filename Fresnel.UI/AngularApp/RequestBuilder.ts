﻿module FresnelApp {

    export class RequestBuilder {


        buildMethodInvokeRequest(method: MethodVM) {
            var request: InvokeMethodRequest = {
                ObjectID: method.ObjectID,
                MethodName: method.InternalName,
                Parameters: []
            };

            for (var i = 0; i < method.Parameters.length; i++) {
                var param = method.Parameters[i];
                var requestParam = {
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

        buildSetPropertyRequest(prop: PropertyVM) {
            var request: SetPropertyRequest = {
                ObjectID: prop.ObjectID,
                PropertyName: prop.InternalName,
                NonReferenceValue: prop.State.Value,
                ReferenceValueId: null
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

    }

}