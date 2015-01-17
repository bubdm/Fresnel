module FresnelApp {

    export class CollectionExplorerController {

        static $inject = ['$rootScope', '$scope', 'fresnelService', 'appService'];

        constructor(
            $rootScope: ng.IRootScopeService,
            $scope: ICollectionExplorerControllerScope,
            fresnelService: IFresnelService,
            appService: AppService) {

            var collection: any = $scope.explorer.__meta;
            
            // This allows Smart-Table to handle the st-safe-src properly:
            collection.DisplayedItems = [].concat(collection.Items);

            $scope.addNewItem = function (itemType: string) {
                var promise = fresnelService.createObject(itemType);

                promise.then((promiseResult) => {
                    var newObject = promiseResult.data.NewObject;

                    appService.identityMap.addObject(newObject);
                    collection.Items.push(newObject);

                    // This will cause the new object to appear in a new Explorer:
                    //$rootScope.$broadcast("showObject", newObject);             
                });

            };

        }

        getEditorTemplate(prop) {
            if (prop.Info == null)
                return;

            switch (prop.Info.Name) {
                case "boolean":
                    switch (prop.Info.PreferredControl) {
                        default:
                            return '/Templates/Editors/booleanRadioEditor.html';
                    }

                case "datetime":
                    switch (prop.Info.PreferredControl) {
                        case "Date":
                            return '/Templates/Editors/dateEditor.html';
                        case "Time":
                            return '/Templates/Editors/timeEditor.html';
                        default:
                            return '/Templates/Editors/dateTimeEditor.html';
                    }

                case "enum":
                    switch (prop.Info.PreferredControl) {
                        case "Checkbox":
                            return '/Templates/Editors/enumCheckboxEditor.html';
                        case "Radio":
                            return '/Templates/Editors/enumRadioEditor.html';
                        default:
                            return '/Templates/Editors/enumSelectEditor.html';
                    }

                case "string":
                    switch (prop.Info.PreferredControl) {
                        case "Password":
                            return '/Templates/Editors/passwordEditor.html';
                        case "TextArea":
                            return '/Templates/Editors/textAreaEditor.html';
                        case "RichTextArea":
                            return '/Templates/Editors/richTextEditor.html';
                        default:
                            return '/Templates/Editors/stringEditor.html';
                    }

                default:
                    return '';
            }
        }

    }
}
