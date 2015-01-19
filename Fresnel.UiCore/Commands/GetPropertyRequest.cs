using System;

namespace Envivo.Fresnel.UiCore.Commands
{
    public class GetPropertyRequest
    {
        public Guid ObjectID { get; set; }

        public string PropertyName { get; set; }
    }
}