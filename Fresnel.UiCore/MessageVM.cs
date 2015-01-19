using System;

namespace Envivo.Fresnel.UiCore.Messages
{
    public class MessageVM
    {
        public DateTime OccurredAt { get; set; }

        public string Text { get; set; }

        public string Detail { get; set; }

        public bool RequiresAcknowledgment { get; set; }

        public bool IsSuccess { get; set; }

        public bool IsInfo { get; set; }

        public bool IsWarning { get; set; }

        public bool IsError { get; set; }
    }
}