using System;
using System.Collections.Generic;

namespace Envivo.Fresnel.SampleModel.Objects
{
    /// <summary>
    ///
    /// </summary>
    public class DetailObject
    {
        private MasterObject _Parent;

        /// <summary>
        ///
        /// </summary>
        public DetailObject()
        {
            this.MoreChildren = new List<DetailObject>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        public DetailObject(MasterObject parent)
            : this()
        {
            this.Parent = parent;
        }

        /// <summary>
        /// The unique ID for this entity
        /// </summary>
        public virtual Guid ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        ///
        /// </summary>
        public virtual IList<DetailObject> MoreChildren { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}