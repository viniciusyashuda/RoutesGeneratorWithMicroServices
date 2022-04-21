namespace TeamAPI.Config
{
    public class TeamDatabaseSettings : ITeamDatabaseSettings
    {
        public string TeamCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
