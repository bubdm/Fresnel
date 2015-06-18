using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class CollectionVM : ObjectVM
    {
        public bool IsCollection { get { return true; } }

        public string ElementType { get; set; }

        public PropertyVM[] ElementProperties { get; set; }

        public ObjectVM[] Items { get; set; }

        public ObjectVM[] DisplayItems { get; set; }

        public InteractionPoint Add { get; set; }

        /// <summary>
        /// Methods for creating the Element Type
        /// </summary>
        public MethodVM[] ElementFactoryMethods { get; set; }

        /// <summary>
        /// Methods for creating all Sub-Classes of the Element Type
        /// </summary>
        public MethodVM[] ElementSubClassFactoryMethods { get; set; }

    }
}