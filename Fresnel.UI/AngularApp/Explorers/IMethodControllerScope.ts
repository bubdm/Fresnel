module FresnelApp {

    export interface IMethodControllerScope extends ng.IScope {

        explorer: Explorer;

        method: MethodVM;

        invoke(method: MethodVM);

        setProperty(param: ValueVM);

        setBitwiseEnumProperty(param: ValueVM, enumValue: number);

        cancel();
    }

}