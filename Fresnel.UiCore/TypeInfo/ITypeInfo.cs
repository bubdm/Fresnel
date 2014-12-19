using Envivo.Fresnel.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.TypeInfo
{
    public interface ITypeInfo
    {
        InputControlTypes PreferredControl { get; set; }
    }
}
