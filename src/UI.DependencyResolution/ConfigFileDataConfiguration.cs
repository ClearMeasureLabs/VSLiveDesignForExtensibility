using System.Configuration;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.UI.DependencyResolution
{
    public class ConfigFileDataConfiguration : IDataConfiguration
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Bootcamp"].ConnectionString;
        }
    }
}