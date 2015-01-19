using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore.Model
{
    public class MethodVM : BaseViewModel
    {
        public Guid ObjectID { get; set; }

        public int Index { get; set; }

        public IEnumerable<ParameterVM> Parameters { get; set; }

        public string MethodName { get; set; }

        public bool IsAsync { get; set; }
    }
}