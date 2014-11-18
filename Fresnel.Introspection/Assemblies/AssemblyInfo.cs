using System;
using System.Reflection;

namespace Envivo.Fresnel.Core.Assemblies
{
    internal class AssemblyInfo
    {

        private readonly string _AssemblyName = string.Empty;
        private readonly string _Title = string.Empty;
        private readonly string _Description = string.Empty;
        private readonly string _Configuration = string.Empty;
        private readonly string _CompanyName = string.Empty;
        private readonly string _Product = string.Empty;
        private readonly string _Copyright = string.Empty;
        private readonly string _Trademark = string.Empty;
        private readonly string _Culture = string.Empty;
        private readonly string _Url = string.Empty;
        private readonly string _Email = string.Empty;
        private readonly string _Version = string.Empty;
        private readonly string _FileVersion = string.Empty;


        public AssemblyInfo()
            : this(Assembly.GetExecutingAssembly())
        {
        }

        public AssemblyInfo(Assembly assembly)
        {
            _AssemblyName = assembly.GetName().Name;

            foreach (Attribute attr in assembly.GetCustomAttributes(false))
            {
                if (attr is AssemblyTitleAttribute)
                {
                    _Title = ((AssemblyTitleAttribute)attr).Title;
                }
                else if (attr is AssemblyDescriptionAttribute)
                {
                    _Description = ((AssemblyDescriptionAttribute)attr).Description;
                }
                else if (attr is AssemblyConfigurationAttribute)
                {
                    _Configuration = ((AssemblyConfigurationAttribute)attr).Configuration;
                }
                else if (attr is AssemblyCompanyAttribute)
                {
                    _CompanyName = ((AssemblyCompanyAttribute)attr).Company;
                }
                else if (attr is AssemblyProductAttribute)
                {
                    _Product = ((AssemblyProductAttribute)attr).Product;
                }
                else if (attr is AssemblyCopyrightAttribute)
                {
                    _Copyright = ((AssemblyCopyrightAttribute)attr).Copyright;
                }
                else if (attr is AssemblyTrademarkAttribute)
                {
                    _Trademark = ((AssemblyTrademarkAttribute)attr).Trademark;
                }
                else if (attr is AssemblyCultureAttribute)
                {
                    _Culture = ((AssemblyCultureAttribute)attr).Culture;
                }
                else if (attr is AssemblyVersionAttribute)
                {
                    _Version = ((AssemblyVersionAttribute)attr).Version;
                }
                else if (attr is AssemblyFileVersionAttribute)
                {
                    _FileVersion = ((AssemblyFileVersionAttribute)attr).Version;
                }
                else if (attr is AssemblyCompanyUrlAttribute)
                {
                    _Url = ((AssemblyCompanyUrlAttribute)attr).Url;
                }
                else if (attr is AssemblyCompanyUrlAttribute)
                {
                    _Url = ((AssemblyCompanyUrlAttribute)attr).Url;
                }
            }
        }

        public string AssemblyName
        {
            get { return _AssemblyName; }
        }

        public string Title
        {
            get { return _Title; }
        }

        public string Description
        {
            get { return _Description; }
        }

        public string Configuration
        {
            get { return _Configuration; }
        }

        public string CompanyName
        {
            get { return _CompanyName; }
        }

        public string Product
        {
            get { return _Product; }
        }

        public string Copyright
        {
            get { return _Copyright; }
        }

        public string Trademark
        {
            get { return _Trademark; }
        }

        public string Culture
        {
            get { return _Culture; }
        }

        public string Url
        {
            get { return _Url; }
        }

        public string Email
        {
            get { return _Email; }
        }

        public string Version
        {
            get { return _Version; }
        }

        public string FileVersion
        {
            get { return _FileVersion; }
        }
    }
}
