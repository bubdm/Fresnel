using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class InvocationAttrbute : Attribute
    {
        public InvocationAttrbute()
        {
            this.PromptForUnsavedChanges = true;
        }

        public InvocationAttrbute(bool mustPrompt)
        {
            this.PromptForUnsavedChanges = mustPrompt;
        }

        public bool PromptForUnsavedChanges { get; set; }
    }
}