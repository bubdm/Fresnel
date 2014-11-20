using System;
using System.Collections.Generic;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Engine.Observers;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.Configuration;

namespace Envivo.Fresnel.Engine
{
    /// <summary>
    /// A dictionary of POCO objects keyed by GUID
    /// </summary>
    public class OuterObjectsIdentifier
    {

        /// <summary>
        /// Returns a list of all Outer objects
        /// </summary>
        /// <param name="levelsToScan">Determines how many levels to scan up the object chain/tree</param>
        public IEnumerable<ObjectObserver> GetOuterObjects(ObjectObserver oObject, int levelsToScan)
        {
            var foundObjects = new Dictionary<object, ObjectObserver>();
            GetOuterObjectsFor(oObject, foundObjects, levelsToScan);
            return foundObjects.Values;
        }

        private void GetOuterObjectsFor(ObjectObserver oObject, Dictionary<object, ObjectObserver> foundObjects, int remainingLevelsToScan)
        {
            if (oObject == null)
                return;

            if (remainingLevelsToScan == 0)
                return;

            // Get all of the Lists that contain the given Object:
            foreach (var oCollection in oObject.OuterCollections)
            {
                if (foundObjects.Contains(oCollection))
                    continue;

                foundObjects.Add(oCollection, oCollection);
            }

            // Get all of the Domain Objects that have references to the given Object:
            foreach (var oProp in oObject.OuterProperties)
            {
                // This prevents bi-dir links to parents from being added to the list:
                var tProp = oProp.TemplateAs<PropertyTemplate>();
                if (tProp.Attributes.Get<ObjectPropertyAttribute>().Relationship == SingleRelationship.OwnedBy)
                    continue;

                var oPropertyOwner = oProp.OuterObject;
                if (foundObjects.Contains(oPropertyOwner))
                    continue;

                foundObjects.Add(oPropertyOwner, oPropertyOwner);

                // Now scan the next level up:
                remainingLevelsToScan--;
                GetOuterObjectsFor(oPropertyOwner, foundObjects, remainingLevelsToScan);
            }
        }


    }
}
