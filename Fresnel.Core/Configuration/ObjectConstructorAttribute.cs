using System;


namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a Domain Object constructor
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Interface)]
    public class ObjectConstructorAttribute : BaseAttribute
    {
        public ObjectConstructorAttribute()
            : base()
        {
            this.CanCreate = true;
        }

        /// <summary>
        /// Determines if the end user can create the object using this constructor.
        /// Useful for preventing Domain Objects with default constructors from being created accidentally.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        internal bool CanCreate { get; set; }

    }

}
