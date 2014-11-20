using Envivo.Fresnel.Engine.Observers;
using Envivo.Fresnel.Introspection.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Engine.Commands
{
    public class ClearCollectionCommand
    {
        private GetCollectionItemsCommand _GetCollectionItemsCommand;

        public ClearCollectionCommand(GetCollectionItemsCommand getCollectionItemsCommand)
        {
            _GetCollectionItemsCommand = getCollectionItemsCommand;
        }

        public void Invoke(CollectionObserver oCollection)
        {
            var items = _GetCollectionItemsCommand.Invoke(oCollection);
            oCollection.UnbindItemsFromCollection(items);

            var oMethod = oCollection.Methods.TryGetValueOrNull("Clear");
            if (oMethod != null)
            {
                var tMethod = oMethod.TemplateAs<MethodTemplate>();
                tMethod.Invoke(oCollection.RealObject, null);
            }
        }

    }
}
