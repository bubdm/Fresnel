module FresnelApp {

    export class AppService {

        identityMap: IdentityMap;

        session: Session;

        mergeMessages(messageSet: any) {
            this.mergeMessageArray(messageSet.Infos, this.session.Messages.Infos);
            this.mergeMessageArray(messageSet.Warnings, this.session.Messages.Warnings);
            this.mergeMessageArray(messageSet.Errors, this.session.Messages.Errors);
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