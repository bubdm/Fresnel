using System;

namespace Envivo.Fresnel.Configuration
{
    /// <summary>
    /// Attributes for a Collection Class
    /// </summary>

    [Serializable()]
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionInstanceAttribute : ObjectInstanceAttribute
    {
    }
}