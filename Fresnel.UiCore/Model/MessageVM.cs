using System;
using System.ComponentModel;
using T4TS;

namespace Envivo.Fresnel.UiCore.Model
{
    [TypeScriptInterface]
    public class MessageVM
    {
        public DateTime OccurredAt { get; set; }

        public string Text { get; set; }

        public string Detail { get; set; }

        public bool RequiresAcknowledgement { get; set; }

        public bool IsSuccess { get; set; }

        [DefaultValue(false)]
        public bool IsInfo { get; set; }

        [DefaultValue(false)]
        public bool IsWarning { get; set; }

        [DefaultValue(false)]
        public bool IsError { get; set; }
    }
}