using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class SetPropertyRequest
    {
        public Guid ObjectID { get; set; }

        public string PropertyName { get; set; }

        public object NonReferenceValue { get; set; }

        public Guid ReferenceValueId { get; set; }
    }
}