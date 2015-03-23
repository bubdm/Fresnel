
module FresnelApp {

    export class SmartTablePredicateService {

        getSortProperty(tableState, collectionProperties: PropertyVM[]): PropertyVM {
            var predicate: string = tableState.sort.predicate;

            if (predicate) {
                var index1 = predicate.indexOf("[");
                var index2 = predicate.indexOf("]");
                var propertyIndex = predicate.substr(index1 + 1, index2 - index1 - 1);

                var sortProp: PropertyVM = collectionProperties[propertyIndex];
                return sortProp;
            }

            return null;
        }
    }

}
