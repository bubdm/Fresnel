﻿using System;
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
using Envivo.Fresnel.SampleModel.Northwind;
using Envivo.Fresnel.DomainTypes.Interfaces;

namespace Fresnel.SampleModel.Persistence
{
    public class ModelConfigurator
    {

        public void ExecuteOn(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .HasMany<Territory>(e => e.Territories)
                .WithMany(t=> t.Employees);

        }

    }
}
