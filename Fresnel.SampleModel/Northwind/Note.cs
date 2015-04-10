using System;
using System.ComponentModel.DataAnnotations;

namespace Envivo.Fresnel.SampleModel.Northwind
{
    public class Note
    {
        private DateTime _CreateAt = DateTime.Now;

        [Key]
        public Guid ID { get; set; }

        public DateTime CreateAt
        {
            get { return _CreateAt; }
            set { _CreateAt = value; }
        }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public override string ToString()
        {
            return typeof(Note).Name;
        }
    }
}