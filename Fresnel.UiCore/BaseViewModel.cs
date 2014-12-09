using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envivo.Fresnel.UiCore
{
    public abstract class BaseViewModel
    {
        public bool IsVisible { get; set; }

        public bool IsEnabled { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CssStyleName { get; set; }

        public string Error { get; set; }

        public string Warning { get; set; }

        public string Tooltip { get; set; }
    }
}
