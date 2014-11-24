using Envivo.Fresnel.Introspection.Templates;
using System;


namespace Envivo.Fresnel.Core.Observers
{

    /// <summary>
    /// The base class for all 'Member' Observers
    /// </summary>
    public abstract class BaseMemberObserver : BaseObserver
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentObject">The ObjectObserver that owns this Member</param>
        /// <param name="memberTemplate">The MemberTemplate that reflects the Member</param>

        internal BaseMemberObserver(ObjectObserver oOuterObject, BaseMemberTemplate tMember)
            : base(oOuterObject.RealObject,
                   oOuterObject.TemplateAs<ClassTemplate>().RealObjectType,
                   tMember)
        {
            this.OuterObject = oOuterObject;
        }

        internal override string DebugID
        {
            get {
                var tClass = this.OuterObject.TemplateAs<ClassTemplate>();

                return string.Concat("[", 
                                this.OuterObject.ID.ToString().ToUpper(), 
                                " ",
                                tClass.FullName, 
                                ",", 
                                this.Template.Name,
                                "]"); 
            }
        }

        /// <summary>
        /// The ObjectObserver that owns this Member
        /// </summary>
        public ObjectObserver OuterObject { get; private set; }

        public string FullName
        {
            get
            {
                var tClass = this.OuterObject.TemplateAs<ClassTemplate>();

                return string.Concat(tClass.FriendlyName,
                                     ".",
                                     this.Template.FriendlyName);
            }
        }

        /// <summary>
        /// The error raised when invoking/accessing this Member
        /// </summary>

        public string ErrorMessage { get; set; }

        public override void Dispose()
        {
            this.OuterObject = null;
            base.Dispose();
        }

        /// <summary>
        /// The time when this Member was last accessed
        /// </summary>
        internal DateTime LastAccessedAtUtc { get; set; }

        ///// <summary>
        ///// Returns TRUE if this Member can be accessed/invoked on another thread
        ///// </summary>

        //internal bool HasThreadSafeAccess()
        //{
        //    // Only the attributes can tell us whether the member can be executed on another thread:
        //    if (this.Template.Attributes.Get<MemberAttribute>().IsThreadSafe == false)
        //    {
        //        return false;
        //    }
        //    else if (this.OuterObject.HasThreadSafeAccess() == false)
        //    {
        //        // If the Object is threadsafe, then by default all of it's members are:
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //internal NullObserver NullObserver
        //{
        //    get
        //    {
        //        if (_oNull == null)
        //        {
        //            _oNull = this.Session.ObserverBuilder.CreateNullObserver();
        //        }
        //        return _oNull;
        //    }
        //}

    }
}
