
using Envivo.Fresnel.Introspection.Assemblies;
using Envivo.Fresnel.Introspection.Configuration;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Introspection.Templates
{

    public class BaseTemplate : ITemplate
    {

        /// <summary>
        /// The AssemblyReader associated with this Template
        /// </summary>
        public AssemblyReader AssemblyReader { get; internal set; }

        /// <summary>
        /// The Name of the Template
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The Name of the Template in a friendly format
        /// </summary>
        public string FriendlyName { get; internal set; }

        /// <summary>
        /// The fully qualified name for the Template
        /// </summary>
        /// <value></value>
        
        
        public string FullName { get; internal set; }

        /// <summary>
        /// All custom Attributes defined for this Template. Values from ClassConfigurations are accessible here.
        /// </summary>
        public AttributesMap Attributes { get; internal set; }

        /// <summary>
        /// Determines if this Template is visible to the user
        /// </summary>
        public bool IsVisible { get; internal set; }

        /// <summary>
        /// The 'Summary' description extracted from the inline code comments
        /// </summary>
        public virtual string Summary { get; internal set; }

        /// <summary>
        /// The 'Remarks' extracted from the inline code comments
        /// </summary>
        public virtual string Remarks { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.Name, " ", this.Summary);
        }

        internal virtual void FinaliseConstruction()
        { 
        }
    }
}
