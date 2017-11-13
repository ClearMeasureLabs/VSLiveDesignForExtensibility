using ClearMeasure.Bootcamp.Core.Model;
using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore;

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
            
        }
    }
}