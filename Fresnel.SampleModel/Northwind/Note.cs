using Envivo.Fresnel.Configuration;
using Envivo.Fresnel.DomainTypes;
using Envivo.Fresnel.SampleModel.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;

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

    }

}