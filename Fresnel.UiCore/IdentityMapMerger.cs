using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore
{
    public class IdentityMapMerger
    {
        public void Merge(IdentityMapDelta delta, IdentityMap targetMap)
        {
            foreach (var item in delta.NewItems)
            {
                targetMap[item.Key] = item.Value;
            }

            foreach (var item in delta.ModifiedItems)
            {
                targetMap[item.Key] = item.Value;
            }

            foreach (var item in delta.RemovedItems)
            {
                targetMap.Remove(item.Key);
            }
        }
    }
}
