using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Envivo.Fresnel.Utils;

namespace System.ComponentModel.DataAnnotations
{

    public class HiddenMembersAttribute : Attribute
    {
        private static Dictionary<string, string> s_FrameworkMemberNameMap = CreateFrameworkMemberNameMap();

        private List<String> _HiddenMemberNames = new List<String>();
        private Dictionary<string, string> _HiddenMemberNamesMap = new Dictionary<string, string>();

        private static Dictionary<string, string> CreateFrameworkMemberNameMap()
        {
            // Note that the list also contains members from the Object:
            var namesToHide = new List<string>();
            namesToHide.AddRange(new string[]
                { "Audit", "DependencyLocator", "IsValid", "IsConsistent",
                    "InnerList", "Load", "Save", "Lock", "Unlock", "Delete"
                });

            namesToHide.AddRange(new string[]
                { "Equals", "ToString", "GetType", "GetHashCode", "Dispose", "Finalize", "Error"
                });

            namesToHide.AddRange(new string[]
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
            foreach (var name in namesToHide)
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

        public IEnumerable<string> Names
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
        public bool Contains(string memberName)
        {
            if (_HiddenMemberNamesMap.Count == 0)
            {
                this.CacheHiddenMemberNames();
            }

            return _HiddenMemberNamesMap.TryGetValueOrNull(memberName.ToLower()) != null;
        }

        /// <summary>
        /// Returns TRUE if the Member with the given name is a Framework member
        /// </summary>
        /// <param name="memberName"></param>
        public bool ContainsFrameworkMember(string memberName)
        {
            return s_FrameworkMemberNameMap.Contains(memberName.ToLower());
        }
    }
}