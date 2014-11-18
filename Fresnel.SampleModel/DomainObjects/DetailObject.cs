using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.Configuration;
using System.Diagnostics;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public class DetailObject
    {
        public Guid ID { get; internal set; }

        public DetailObject()
        {
            this.ID = Guid.NewGuid();
            this.MoreChildren = new List<DetailObject>();
        }

        public DetailObject(MasterObject parent)
            : this()
        {
            this.Parent = parent;
        }

        //public override bool Equals(object obj)
        //{
        //    if (object.ReferenceEquals(this, obj))
        //        return true;

        //    var that = obj as DetailObject;
        //    if (that == null)
        //        return false;

        //    return this.ID.Equals(that.ID);
        //}

        //public override int GetHashCode()
        //{
        //    return this.ID.GetHashCode();
        //}

        public string Name { get; set; }

        private MasterObject _Parent;

        public MasterObject Parent
        {
            get { return _Parent; }
            internal set
            {
                if (_Parent != value)
                {
                    if (_Parent != null)
                    {
                        _Parent.Children.Remove(this);
                    }
                    _Parent = value;
                    if (_Parent != null)
                    {
                        _Parent.Children.Add(this);
                    }
                }
            }
        }

        public IList<DetailObject> MoreChildren { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
