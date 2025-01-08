namespace PokemonApi.Database
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string PokemonCollectionName { get; set; } = "Pokemon";
    }
}
