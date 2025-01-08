using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using PokemonApi.Database;

namespace PokemonApi.Services
{
    public class MongoDbService : IPokemonService
    {
        private readonly IMongoCollection<Pokemon> _pokemonCollection;

        public MongoDbService(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _pokemonCollection = database.GetCollection<Pokemon>(settings.PokemonCollectionName);
        }

        public async Task<List<Pokemon>> GetAllAsync() =>
            await _pokemonCollection.Find(new BsonDocument()).ToListAsync();

        public async Task<Pokemon> GetByIdAsync(int id) =>
            await _pokemonCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Pokemon pokemon) =>
            await _pokemonCollection.InsertOneAsync(pokemon);

        public async Task UpdateAsync(Pokemon pokemon) =>
            await _pokemonCollection.ReplaceOneAsync(p => p.Id == pokemon.Id, pokemon);

        public async Task DeleteAsync(int id) =>
            await _pokemonCollection.DeleteOneAsync(p => p.Id == id);
    }
}
