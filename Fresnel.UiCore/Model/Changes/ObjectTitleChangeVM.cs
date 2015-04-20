using Envivo.Fresnel.UiCore.Model;
using System;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model.Changes
{
    [TypeScriptInterface]
    public class ObjectTitleChangeVM
    {
        public Guid ObjectID { get; set; }

        public string Title { get; set; }

    }
}