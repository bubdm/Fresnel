module FresnelApp {

    export class AppService {

        identityMap: IdentityMap;

        mergeMessages(messages: any, target: Session) {
            for (var i = 0; i < messages.length; i++) {
                target.Messages.push(messages[i]);
            }
        }

    }

}