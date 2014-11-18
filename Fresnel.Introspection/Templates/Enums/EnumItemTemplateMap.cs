using System;
using System.Collections.Generic;
using System.Linq;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class EnumItemTemplateMap : ReadOnlyDictionary<string, EnumItemTemplate>
    {

        //private List<EnumItemTemplate> _tEnumItems;

        public EnumItemTemplateMap(IDictionary<string, EnumItemTemplate> items)
            :base(items)
        {
            //_tEnumItems = results.Values.ToList();
        }

        //internal EnumItemTemplate FirstItem
        //{
        //    get { return _tEnumItems[0]; }
        //}

        //internal EnumItemTemplate LastItem
        //{
        //    get { return _tEnumItems[_tEnumItems.Count - 1]; }
        //}

    }

}
