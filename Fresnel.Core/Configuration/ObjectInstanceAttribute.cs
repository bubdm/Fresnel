using System;
using System.Collections.Generic;
using Envivo.Fresnel.Utils;

namespace Envivo.Fresnel.Core.Configuration
{

    /// <summary>
    /// Attributes for a Domain Object Class
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ObjectInstanceAttribute : BaseAttribute
    {
        private static Dictionary<string, string> s_FrameworkMemberNameMap = CreateFrameworkMemberNameMap();

        private List<String> _HiddenMemberNames = new List<String>();
        private Dictionary<string, string> _HiddenMemberNamesMap = new Dictionary<string, string>();

        private string _IdPropertyName;
        private string _VersionPropertyName;

        public ObjectInstanceAttribute()
            : base()
        {
            this.IsCreatable = true;
            this.IsPersistable = true;
            this.IsVisible = true;
            this.IsLazyLoaded = true;
            this.IsImmutable = false;
            this.IsThreadSafe = true;

            this.MemberDisplayOrder = new string[] { };
            this.CategoryDisplayOrder = new string[] { };

            this.IdPropertyName = "Id";
            this.VersionPropertyName = "Version";
        }

        private static Dictionary<string, string> CreateFrameworkMemberNameMap()
        {
            // Note that the list also contains members from the Object:
            var memberNamesToHide = new List<string>();
            memberNamesToHide.AddRange(new string[]
                { "Audit", "DependencyLocator", "IsValid", "IsConsistent", 
                    "InnerList", "Load", "Save", "Lock", "Unlock", "Delete" 
                });

            memberNamesToHide.AddRange(new string[]
                { "Equals", "ToString", "GetType", "GetHashCode", "Dispose", "Finalize", "Error" 
                });

            memberNamesToHide.AddRange(new string[]
                { "Add", "Remove", "Contains", "Clear", "Count", "GetEnumerator", "CopyTo", "Comparer",
                    "Keys", "Values", "ContainsKey", "ContainsValue",
                    "GetObjectData", "OnDeserialization", "TryGetValue", 
                    "AddRange", "AsReadOnly", "BinarySearch", "ConvertAll", "Exists", "Find",
                    "FindAll", "FindIndex", "FindLast", "FindLastIndex", "ForEach", "GetRange", 
                    "IndexOf", "Insert", "InsertRange", "IsReadOnly", "LastIndexOf", "List",
                    "RemoveAll", "RemoveAt", "RemoveRange", "Reverse", "Sort",
                    "ToArray", "TrimExcess", "TrueForAll", "Capacity",
                    "RaiseListChangedEvents", "AllowNew", "AllowEdit", "AllowRemove",
                    "ResetBindings", "ResetItem", "AddNew", "CancelNew", "EndNew", "OnPropertyChanged"
                });

            var results = new Dictionary<string, string>();
            foreach (var name in memberNamesToHide)
            {
                results.Add(name.ToLower(), name);
            }
            return results;
        }

        private void CacheHiddenMemberNames()
        {
            foreach (var name in _HiddenMemberNames)
            {
                _HiddenMemberNamesMap[name.ToLower()] = name;
            }
        }

        /// <summary>
        /// Determines if the object can be created by the end user
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsCreatable { get; set; }


        /// <summary>
        /// Determines if the object can be persisted
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsPersistable { get; set; }

        /// <summary>
        /// Determines whether all object properties are loaded only when explicitly requested by the user. Defaults to TRUE.
        /// Note that this only takes effect on properties that contain Domain Objects or Lists.
        /// If set to FALSE, all Object/list properties are Eager loaded.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsLazyLoaded { get; set; }

        /// <summary>
        /// Determines whether the Object remains in memory for the lifetime of the session.
        /// Use this with objects that are highly requested and are immutable.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsStaticData { get; set; }

        /// <summary>
        /// Determines whether persistant Object's can be modified
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsImmutable { get; set; }

        /// <summary>
        /// Determines whether access to this member can be executed on a separate thread.
        /// Set this attribute to FALSE if the Object accesses a resource that is not thread safe (e.g. Windows controls).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsThreadSafe { get; set; }

        /// <summary>
        /// Determines whether the user can elect to lock the object prior to editing it
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        internal bool AllowPessimisticLocking { get; set; }

        /// <summary>
        /// Determines whether the object is forcefully locked as soon as it is edited
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        internal bool ForcePessimisticLocking { get; set; }

        /// <summary>
        /// A list of Member names in the order that should be displayed to the end user
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] MemberDisplayOrder { get; set; }

        /// <summary>
        /// A list of Category names in the order that should be displayed to the end user
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] CategoryDisplayOrder { get; set; }

        /// <summary>
        /// A list of Member names that should not be displayed to the end user
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string[] HiddenMembers
        {
            get { return _HiddenMemberNames.ToArray(); }
            set
            {
                // Instead of replacing the existing list, we'll append to it:
                _HiddenMemberNames.AddRange(value);
                foreach (var name in value)
                {
                    _HiddenMemberNamesMap[name.ToLower()] = name;
                }
            }
        }

        /// <summary>
        /// Returns TRUE if the Member with the given name should be hidden
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        internal bool HasHiddenMemberNamed(string memberName)
        {
            if (_HiddenMemberNamesMap.Count == 0)
            {
                this.CacheHiddenMemberNames();
            }

            return _HiddenMemberNamesMap.Contains(memberName.ToLower());
        }

        /// <summary>
        /// Returns TRUE if the Member with the given name is a Framework member
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        internal bool HasFrameworkMemberCalled(string memberName)
        {
            return s_FrameworkMemberNameMap.Contains(memberName.ToLower());
        }

        /// <summary>
        /// Determines whether all Properties are hidden from the end user
        /// </summary>
        public bool HideAllProperties { get; set; }


        /// <summary>
        /// Determines whether all Methods are hidden from the end user
        /// </summary>
        public bool HideAllMethods { get; set; }

        /// <summary>
        /// The name of the property that stores the Domain Object's ID.  The Domain Object's property must be GUID.
        /// </summary>
        public string IdPropertyName
        {
            get { return _IdPropertyName; }
            set
            {
                _IdPropertyName = value;
                _HiddenMemberNames.Add(_IdPropertyName);
                _HiddenMemberNamesMap[_IdPropertyName.ToLower()] = _IdPropertyName;
            }
        }


        /// <summary>
        /// The name of the property that stores the Domain Object's Version.  The Domain Object's property must be int64.
        /// </summary>
        public string VersionPropertyName
        {
            get { return _VersionPropertyName; }
            set
            {
                _VersionPropertyName = value;
                _HiddenMemberNames.Add(_VersionPropertyName);
                _HiddenMemberNamesMap[_VersionPropertyName.ToLower()] = _VersionPropertyName;
            }
        }

    }

}
