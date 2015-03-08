using Envivo.Fresnel.DomainTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    ///
    /// </summary>
    public class ValidationExample : IValidatable
    {
        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Please enter the person's Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Please enter the person's Age
        /// </summary>
        public virtual int Age { get; set; }

        private Dictionary<string, string> _Errors = new Dictionary<string, string>();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            _Errors.Clear();
            // Run through the checks here:

            if (string.IsNullOrEmpty(this.Name))
            {
                _Errors.Add("Name", "Please provide a name");
            }
            else if (this.Name.Length == 1)
            {
                _Errors.Add("Name", "Please provide a valid name");
            }

            if (this.Age < 16 || this.Age > 80)
            {
                _Errors.Add("Age", "Person must be between 16 and 80 years old to qualify");
            }

            return string.IsNullOrEmpty(((IValidatable)this).Error);
        }

        string System.ComponentModel.IDataErrorInfo.Error
        {
            get
            {
                if (_Errors.Count == 0)
                    return null;

                var temp = new string[_Errors.Count];
                _Errors.Values.CopyTo(temp, 0);
                return "The following problems we identified:" +
                Environment.NewLine +
                string.Join(Environment.NewLine, temp);
            }
        }

        string System.ComponentModel.IDataErrorInfo.this[string columnName]
        {
            get
            {
                string error;
                _Errors.TryGetValue(columnName, out error);
                return error;
            }
        }
    }
}