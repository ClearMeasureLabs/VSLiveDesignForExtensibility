using ClearMeasure.Bootcamp.Core.Model;
using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class ManagerMap : SubclassMap<Manager>, IEntityFrameworkMapping
    {
        public ManagerMap()
        {
            Not.LazyLoad();
            References(x => x.AdminAssistant).Column("AdminAssistantId");
            DiscriminatorValue("MGR");
        }

        public void Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<Manager>();
            mapping.HasBaseType<Employee>();
            mapping.HasDiscriminator<string>("Type").HasValue("MGR");
            mapping.HasOne(x => x.AdminAssistant);
        }
    }
}