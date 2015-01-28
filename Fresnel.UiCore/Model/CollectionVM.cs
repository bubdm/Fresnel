using System.Collections.Generic;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class CollectionVM : ObjectVM
    {
        public bool IsCollection { get { return true; } }

        public string ElementType { get; set; }

        public IEnumerable<SettableMemberVM> ElementProperties { get; set; }

        public IEnumerable<ObjectVM> Items { get; set; }

        public IEnumerable<ObjectVM> DisplayItems { get; set; }
    }
}