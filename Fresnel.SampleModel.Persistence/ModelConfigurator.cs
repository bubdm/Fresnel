using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using Envivo.Fresnel.Core.Persistence;
using System.Data.Entity.Core;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Envivo.Fresnel.SampleModel.Objects;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Fresnel.SampleModel.Persistence
{
    public class ModelConfigurator
    {

        public void ExecuteOn(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<MasterObject>()
                .HasMany<DetailObject>(x => x.Children)
                .WithRequired(x=> x.Parent);

            //modelBuilder
            //    .Entity<DetailObject>()
            //    .Property(x => x.Version)
            //    .IsConcurrencyToken();
        }

    }
}
