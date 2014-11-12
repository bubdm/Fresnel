using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.Services
{
    public class Service2 : IDomainService
    {

        public void UploadFile(string file)
        {
            
        }

        public string DownloadFile()
        {
            return "Done!";
        }
    }

    public class Service3 : IDomainService
    {

        public void UploadFile(string file)
        {

        }

        public string DownloadFile()
        {
            return "Done!";
        }
    }
}
