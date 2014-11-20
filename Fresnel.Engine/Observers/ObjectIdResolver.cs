using Envivo.Fresnel.Introspection;
using System;
using System.Collections.Generic;
using System.Linq;
using Envivo.Fresnel.Utils;
using Envivo.Fresnel.Introspection.Templates;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.Engine.Observers
{

    /// <summary>
    /// Returns Observers for .NET objects & values
    /// </summary>
    public class ObjectIdResolver
    {
       
        public object GetId(object obj, ClassTemplate tClass)
        {
            if (obj == null)
                return Guid.NewGuid();

            var entity = obj as IEntity;
            if (entity != null)
            {
                var result = entity.ID;
                return result;
            }

            if (tClass.IdProperty != null)
            {
                var result = tClass.IdProperty.GetProperty(obj);
                return result;
            }

            var msg = string.Concat("Unable to determine ID for ", tClass.FriendlyName);
            throw new FresnelException(msg);
        }
    }

}
