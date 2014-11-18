using System.Collections.Generic;
using System.Reflection;

namespace Envivo.Fresnel.Configuration
{
    public interface IApplicationConfiguration
    {

        string SupportEmailAddress { get; set; }

        string CustomLicenceMessage { get; set; }

    }
}
