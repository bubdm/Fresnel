﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore.Objects
{
    public class MethodVM : BaseViewModel
    {
        //public Guid ObjectID { get; set; }

        public IEnumerable<ParameterVM> Parameters { get; set; }

    }
}