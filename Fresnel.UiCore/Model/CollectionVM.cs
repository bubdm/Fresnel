using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Model
{
    public class CollectionVM : ObjectVM
    {
        public bool IsCollection { get { return true; } }

        public string ElementType { get; set; }

        public IEnumerable<ValueVM> ElementProperties { get; set; }

        public IEnumerable<ObjectVM> Items { get; set; }


    }
}