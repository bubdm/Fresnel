using System;


namespace Envivo.Fresnel.Core.Assemblies
{

    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyCompanyEmailAttribute : System.Attribute
    {

        public AssemblyCompanyEmailAttribute(string email)
        {
            this.Email = email;
        }

        public string Email { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyCompanyUrlAttribute : System.Attribute
    {

        public AssemblyCompanyUrlAttribute(string url)
        {
            this.Url = url;
        }

        public string Url { get; private set; }
    }

}
