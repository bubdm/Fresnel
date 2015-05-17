using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public interface IUndoCommand
    {
        bool CanHandle(IFresnelEvent eventToUndo);

        ActionResult Invoke(IFresnelEvent eventToUndo);
    }

}