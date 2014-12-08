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
        private MasterObject _Parent;

        public DetailObject()
        {
            this.MoreChildren = new List<DetailObject>();
        }

        public DetailObject(MasterObject parent)
            : this()
        {
            this.Parent = parent;
        }

        public virtual Guid ID { get; set; }

        public virtual string Name { get; set; }

        public virtual MasterObject Parent
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

        public virtual IList<DetailObject> MoreChildren { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
