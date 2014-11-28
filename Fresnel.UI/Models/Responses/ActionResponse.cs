using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fresnel.UI.Models.Responses;

namespace Fresnel.UI.Models.CommandResponses
{
    public class ActionResponse
    {

        public bool Passed { get; set; }

        public bool Failed { get; set; }

        public bool HasWarning { get; set; }

        public string InfoMessage { get; set; }

        public string WarningMessage { get; set; }

        public string ErrorMessage { get; set; }

        public ObjectCreation[] Creations { get; set; }

        public ObjectModification[] Modifications { get; set; }

        public ObjectRemoval[] Removals { get; set; }

    }
}