using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;using Envivo.DomainTypes;
using Envivo.TrueView.Domain.Attributes;

namespace Envivo.Sample.Model.Services
{
    public class Service1 : IDomainService
    {

        /// <summary>
        /// Uploads a file to the server
        /// </summary>
        /// <param name="file">Provide the file to be uploaded to the server</param>
        public void UploadFile(string file)
        {
            
        }

        public string DownloadFile()
        {
            return "Done!";
        }

    }
}
