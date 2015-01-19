using System.Collections.Generic;

namespace Envivo.Fresnel.Introspection.Templates
{
    public class ClassHierarchyDepthComparer : IComparer<ClassTemplate>
    {
        public int Compare(ClassTemplate x, ClassTemplate y)
        {
            var difference = x.InheritanceDepth - y.InheritanceDepth;
            if (difference == 0)
            {
                // The depths are the same, so compare by name instead:
                difference = string.Compare(x.Name, y.Name);
            }

            return difference;
        }
    }
}