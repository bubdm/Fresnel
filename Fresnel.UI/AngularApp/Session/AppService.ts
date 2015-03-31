module FresnelApp {

    export class AppService {

        identityMap: IdentityMap;

        mergeMessages(messages: MessageVM[], target: SessionVM) {
            for (var i = 0; i < messages.length; i++) {
                target.Messages.push(messages[i]);
            }
        }

    }

}