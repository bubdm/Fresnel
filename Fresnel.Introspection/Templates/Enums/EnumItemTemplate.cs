
using System.Reflection;
using Envivo.Fresnel.Utils;


namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// A Template that represents an Enumeration
    /// </summary>
    
    public class EnumItemTemplate : BaseTemplate
    {
        public object Value {get; internal set;}

        public override string ToString()
        {
            return this.FriendlyName;
        }

    }
}
