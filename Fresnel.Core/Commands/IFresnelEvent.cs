using Envivo.Fresnel.Core.ChangeTracking;
using Envivo.Fresnel.Core.Observers;
using Envivo.Fresnel.Introspection;
using Envivo.Fresnel.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Envivo.Fresnel.Core.Commands
{
    public interface IFresnelEvent
    {
        DateTime OccurredAt { get; }

        long SequenceNo { get; }

        IEnumerable<ObjectObserver> AffectedObjects { get; }

        ActionResult Do();

        ActionResult Undo();
    }

}