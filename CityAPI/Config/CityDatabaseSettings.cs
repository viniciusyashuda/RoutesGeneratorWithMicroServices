namespace CityAPI.Config
{
    public class CityDatabaseSettings : ICityDatabaseSettings
    {
        public string CityCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
