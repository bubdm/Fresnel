module FresnelApp {

    export class AppService {

        identityMap: IdentityMap;

        mergeMessages(messageSet: any, target: Session) {
            this.mergeMessageArray(messageSet.Infos, target.Messages.Infos);
            this.mergeMessageArray(messageSet.Warnings, target.Messages.Warnings);
            this.mergeMessageArray(messageSet.Errors, target.Messages.Errors);
        }

        private mergeMessageArray(sourceArray: any[], targetArray: any[]) {
            if (sourceArray) {
                for (var i = 0; i < sourceArray.length; i++) {
                    var message = sourceArray[i];
                    targetArray.push(message);
                }
            }
        }
    }

}