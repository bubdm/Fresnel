using Envivo.Fresnel.Configuration;
using Newtonsoft.Json;
using System.Reflection;

namespace Envivo.Fresnel.Introspection.Templates
{
    /// <summary>
    /// The base class for all 'Member' Templates
    /// </summary>
    /// <remarks>This class acts as a wrapper to a .NET class Member</remarks>
    public abstract class BaseMemberTemplate : BaseTemplate
    {
        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="tOuterClass">The ClassTemplate that owns this Member</param>
        ///// <param name="memberInfo">The MemberInfo</param>
        //
        //public BaseMemberTemplate(ClassTemplate tOuterClass,
        //                          MemberInfo memberInfo,
        //                          AttributesMap memberAttributes)
        //    : base(memberInfo.Name)
        //{
        //    _tOuterClass = tOuterClass;
        //    this.MemberInfo = memberInfo;
        //    this.Attributes = memberAttributes;

        //}

        internal override void FinaliseConstruction()
        {
            base.FinaliseConstruction();

            if (this.Attributes.Count > 0)
            {
                var memberAttr = this.Attributes.Get<MemberAttribute>();
                this.IsVisible = memberAttr.IsVisible;
            }

            // We don't want hidden members to be visible:
            var memberName = this.Name;
            var attr = this.OuterClass.Attributes.Get<ObjectInstanceAttribute>();
            if (attr.HasHiddenMemberNamed(memberName))
            {
                this.IsVisible = false;
            }

            // We also don't want framework members to be visible:
            if (attr.HasFrameworkMemberCalled(memberName))
            {
                this.IsVisible = false;
                this.IsFrameworkMember = true;
            }

            //if (this.Attributes.Count > 0)
            //{
            //    var memberAttr = this.Attributes.Get<MemberAttribute>();
            //    this.IsVisible = memberAttr.IsVisible;
            //}
        }

        /// <summary>
        /// The ClassTemplate that owns this Member
        /// </summary>
        [JsonIgnore]
        public ClassTemplate OuterClass { get; internal set; }

        [JsonIgnore]
        public MemberInfo MemberInfo { get; internal set; }

        public bool IsFrameworkMember { get; internal set; }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}