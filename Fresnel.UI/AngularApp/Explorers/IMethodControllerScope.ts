module FresnelApp {

    export interface IMethodControllerScope extends ng.IScope {

        explorer: Explorer;

        method: MethodVM;

        invoke(method: MethodVM);

        setProperty(param: ParameterVM);

        setBitwiseEnumProperty(param: ParameterVM, enumValue: number);

        cancel();
    }

}