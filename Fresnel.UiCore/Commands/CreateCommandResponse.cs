﻿using Envivo.Fresnel.UiCore.Model;
using T4TS;

namespace Envivo.Fresnel.UiCore.Commands
{
    [TypeScriptInterface]
    public class CreateCommandResponse : BaseCommandResponse
    {
        public ObjectVM NewObject { get; set; }
    }
}