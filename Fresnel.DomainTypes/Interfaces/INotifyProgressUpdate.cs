
using System.Collections.Generic;
using System.Text;

namespace Envivo.Fresnel.DomainTypes.Interfaces
{

    public delegate void ReportProgressEventHandler(object sender,IProgressArgs e);

    public interface IProgressReporter
    {
        event ReportProgressEventHandler NotifyProgressUpdate;
    }

}
