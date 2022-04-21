namespace TeamAPI.Config
{
    public interface ITeamDatabaseSettings
    {
        string TeamCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
