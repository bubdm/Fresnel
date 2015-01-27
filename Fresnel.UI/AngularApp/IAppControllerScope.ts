module FresnelApp {

    export interface IAppControllerScope extends ng.IScope {

        identityMap: IdentityMap;

        session: SessionVM;

        loadSession();

        IsModalVisible: boolean;
    }

}