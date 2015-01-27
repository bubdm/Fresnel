module FresnelApp {

    export class RequestBuilder {


        buildMethodInvokeRequest(method: any) {
            var request = {
                ObjectId: method.ObjectID,
                MethodName: method.MethodName,
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

        buildSetPropertyRequest(prop: any) {
            var request = {
                ObjectId: prop.ObjectID,
                PropertyName: prop.PropertyName,
                NonReferenceValue: prop.State.Value
            };

            return request;
        }

        buildGetObjectRequest(obj: IObjectVM) {
            var request = {
                ObjectId: obj.ID,
            };

            return request;
        }

        buildGetPropertyRequest(prop: any) {
            var request = {
                ObjectId: prop.ObjectID,
                InternalName: prop.InternalName
            };

            return request;
        }
    }

}