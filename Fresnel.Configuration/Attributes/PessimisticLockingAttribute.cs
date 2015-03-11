using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.ComponentModel.DataAnnotations
{

    [AttributeUsage(AttributeTargets.Property)]
    public class PessimisticLockingAttribute : Attribute
    {
        /// <summary>
        /// The current user is allowed to lock the object before editing, thus preventing other users from modifying it
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// The object is locked before editing, preventing other users from modifying it
        /// </summary>
        public bool IsForced { get; set; }

    }
}