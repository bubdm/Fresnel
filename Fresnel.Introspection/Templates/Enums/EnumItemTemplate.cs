


using System;
namespace Envivo.Fresnel.Introspection.Templates
{

    /// <summary>
    /// A Template that represents an Enumeration
    /// </summary>

    public class EnumItemTemplate : BaseTemplate
    {
        public EnumItemTemplate(EnumTemplate tParentEnum)
        {
            _XmlComments = new Lazy<XmlComments>(
                        () => this.AssemblyReader.XmlDocReader.GetXmlCommentsFor(tParentEnum, this),
                        System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public object Value { get; internal set; }

        public int NumericValue { get; internal set; }

        public override string ToString()
        {
            return this.FriendlyName;
        }

    }
}
