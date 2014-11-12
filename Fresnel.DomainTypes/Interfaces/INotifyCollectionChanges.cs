
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{

    public delegate void NotifyCollectionChangesEventHandler<T>(object sender,ICollectionChangeEventArgs<T> e)
        where T : class;


    public interface INotifyCollectionChanges<T>
        where T : class
    {

        event NotifyCollectionChangesEventHandler<T> Changing;

        event NotifyCollectionChangesEventHandler<T> Changed;

    }

}
