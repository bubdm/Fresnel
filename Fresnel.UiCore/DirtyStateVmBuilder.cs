using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Core.Permissions;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.UiCore.Model.TypeInfo;
using Envivo.Fresnel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Envivo.Fresnel.UiCore
{
    public class DirtyStateVmBuilder
    {

        public DirtyStateVM BuildFor(ObjectObserver oObject)
        {
            return new DirtyStateVM()
            {
                ObjectID = oObject.ID,
                IsTransient = oObject.ChangeTracker.IsTransient,
                IsPersistent = oObject.ChangeTracker.IsPersistent,
                IsDirty = oObject.ChangeTracker.IsDirty,
                HasDirtyChildren = oObject.ChangeTracker.HasDirtyObjectGraph,
            };
        }
    }
}