namespace Envivo.Fresnel.SampleModel.TestTypes
{
    public class ClassWithHiddenCtor
    {
        internal ClassWithHiddenCtor()
        { }

        /// <summary>
        /// The name of this object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description for this object
        /// </summary>
        public string Description { get; set; }
    }
}