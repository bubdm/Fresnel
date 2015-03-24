module FresnelApp {

    export interface IMethodScope extends ng.IScope {

        explorer: Explorer;

        method: MethodVM;

        invoke(method: MethodVM);

        setProperty(param: ParameterVM);

        setBitwiseEnumProperty(param: ParameterVM, enumValue: number);

        isBitwiseEnumPropertySet(param: ParameterVM, enumValue: number): boolean

        associate(param: ParameterVM);

        addExistingItems(collectionParam: ParameterVM, coll: CollectionVM);

        setParameterOnServer(param: ParameterVM);

        close(explorer: Explorer);
    }

}