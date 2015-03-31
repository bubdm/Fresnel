using Envivo.Fresnel.UiCore.Model;

using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.Changes
{
    [TypeScriptInterface]
    public class ModificationsVM
    {
        public ObjectVM[] NewObjects { get; set; }

        public PropertyChangeVM[] PropertyChanges { get; set; }

        public ObjectTitleChangeVM[] ObjectTitleChanges { get; set; }

        public CollectionElementVM[] CollectionAdditions { get; set; }

        public CollectionElementVM[] CollectionRemovals { get; set; }

        public ParameterChangeVM[] MethodParameterChanges { get; set; }

        public ObjectVM[] SavedObjects { get; set; }
    }
}