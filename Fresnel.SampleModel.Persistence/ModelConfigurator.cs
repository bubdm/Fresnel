using Envivo.Fresnel.SampleModel.Northwind;
using System.Data.Entity;

namespace Fresnel.SampleModel.Persistence
{
    public class ModelConfigurator
    {
        public void ExecuteOn(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .HasMany<Territory>(e => e.Territories)
                .WithMany(t => t.Employees);
        }
    }
}