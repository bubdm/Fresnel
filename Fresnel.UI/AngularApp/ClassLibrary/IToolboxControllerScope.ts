﻿module FresnelApp {

    export interface IToolboxControllerScope extends ng.IScope {

        classHierarchy: ClassItem[];

        create(fullyQualifiedName: string);

        loadClassHierarchy();

    }

}