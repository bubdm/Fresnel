﻿using System;
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

        public bool IsInfo { get; set; }

        public bool IsWarning { get; set; }

        public bool IsError { get; set; }
    }
}