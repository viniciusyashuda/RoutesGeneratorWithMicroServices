namespace CityAPI.Config
{
    public interface ICityDatabaseSettings
    {
        string CityCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
