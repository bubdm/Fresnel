
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using System;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Envivo.Fresnel.DomainTypes
{
    /// <summary>
    /// Any object within a Domain that ha unique identity.
    /// It may be necessary to override Equals() so that comparisons are made by ID.
    /// </summary>
    [Serializable]
    public abstract partial class BaseDomainObject : IDomainObject,
                                                     INotifyPropertyChanged,
                                                     IDataErrorInfo,
                                                     IValidatable,
                                                     IDisposable
    {

        private Guid _ID = Guid.NewGuid();
        private long _Version = -1;
        private IAudit _Audit = new Audit();

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var that = obj as BaseDomainObject;
            if (that == null)
                return false;

            return this.ID.Equals(that.ID);
        }

        public override int GetHashCode()
        {
            return _ID.GetHashCode();
        }

        /// <summary>
        /// A unique identifier used to track this object
        /// </summary>
        public virtual Guid ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public virtual long Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        public virtual IAudit Audit
        {
            get { return _Audit; }
            set { _Audit = value; }
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        virtual protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged == null)
                return;

            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Helper method to set a property value and raise the PropertyChanged event
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingField"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <example>this.Set(ref _Name, value, "Name");
        /// </example>
        /// <returns></returns>
        virtual protected bool Set<T>(ref T backingField, T newValue, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
                return false;

            backingField = newValue;
            this.OnPropertyChanged(propertyName);

            return true;
        }


        private Dictionary<string, string> _ErrorMap = new Dictionary<string, string>();
        private string _ErrorMessage;

        Dictionary<string, string> Errors
        {
            get { return _ErrorMap; }
        }

        string IDataErrorInfo.Error
        {
            get { return _ErrorMessage; }
        }

        string IDataErrorInfo.this [string propertyName]
        {
            get
            {
                string error = null;
                this.Errors.TryGetValue(propertyName, out error);
                return error;
            }
        }

        protected virtual void SetError(string propertyName, string errorDescription)
        {
            var errors = this.Errors;
            this.Errors[propertyName] = errorDescription;
            this.UpdateErrorMessage();
        }

        private void UpdateErrorMessage()
        {
            switch (this.Errors.Count)
            {
                case 0:
                    _ErrorMessage = string.Empty;
                    break;

                case 1:
                    _ErrorMessage = this.Errors.ElementAt(0).Value;
                    break;

                default:
                    var sb = new StringBuilder("The following problems were found:\n");
                    foreach (var item in _ErrorMap.Where(p => string.IsNullOrEmpty(p.Value) == false))
                    {
                        sb.AppendLine(string.Concat(" ", item.Key, " : ", item.Value));
                    }
                    _ErrorMessage = sb.ToString();
                    break;
            }
        }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual void Dispose()
        {
            _ErrorMap = null;
        }

    }
}
