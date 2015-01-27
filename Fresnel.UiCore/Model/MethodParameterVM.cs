using Envivo.Fresnel.UiCore.TypeInfo;
using System;

namespace Envivo.Fresnel.UiCore.Model
{
    public class MethodParameterVM : BaseViewModel
    {
        public Guid ObjectID { get; set; }

        public string MethodName { get; set; }

        public string ParameterName { get; set; }

        public int Index { get; set; }

        public bool IsObject { get; set; }

        public bool IsCollection { get; set; }

        public bool IsNonReference { get; set; }

        public ITypeInfo Info { get; set; }

        public MethodParameterState State { get; set; }
    }
}