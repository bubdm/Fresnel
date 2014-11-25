
namespace Envivo.Fresnel.Introspection.Templates
{

    public class StaticMethodTemplateMapBuilder
    {
        private StaticMethodInfoMapBuilder _StaticMethodInfoMapBuilder;
        private MethodTemplateMapBuilder _MethodTemplateMapBuilder;

        public StaticMethodTemplateMapBuilder
        (
            StaticMethodInfoMapBuilder staticMethodInfoMapBuilder,
            MethodTemplateMapBuilder methodTemplateMapBuilder
        )
        {
            _StaticMethodInfoMapBuilder = staticMethodInfoMapBuilder;
            _MethodTemplateMapBuilder = methodTemplateMapBuilder;
        }

        public MethodTemplateMap BuildFor(ClassTemplate tClass)
        {
            var staticMethodInfoMap = _StaticMethodInfoMapBuilder.BuildFor(tClass.RealObjectType);
            var result = _MethodTemplateMapBuilder.BuildFrom(tClass, staticMethodInfoMap);
            return result;
        }


    }

}
