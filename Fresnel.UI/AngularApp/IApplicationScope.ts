module FresnelApp {

    export interface IApplicationScope extends ng.IScope {

        identityMap: IdentityMap;

        session: SessionVM;

        loadSession();

        IsModalVisible: boolean;
    }

}