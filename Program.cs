using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MongoDB.Driver;
using PokemonApi.Database;
using PokemonApi.Services;
using DotNetEnv;

namespace PokemonApi
{
    // Pokemon Model
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Ability { get; set; }
        public int Level { get; set; }
    }

    // Interface for Pokemon Service
    public interface IPokemonService
    {
        Task<List<Pokemon>> GetAllAsync();
        Task<Pokemon> GetByIdAsync(int id);
        Task AddAsync(Pokemon pokemon);
        Task UpdateAsync(Pokemon pokemon);
        Task DeleteAsync(int id);
    }

    // MongoDbService Implementation
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
            await _pokemonCollection.Find(Builders<Pokemon>.Filter.Empty).ToListAsync();

        public async Task<Pokemon> GetByIdAsync(int id) =>
            await _pokemonCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Pokemon pokemon) =>
            await _pokemonCollection.InsertOneAsync(pokemon);

        public async Task UpdateAsync(Pokemon pokemon) =>
            await _pokemonCollection.ReplaceOneAsync(p => p.Id == pokemon.Id, pokemon);

        public async Task DeleteAsync(int id) =>
            await _pokemonCollection.DeleteOneAsync(p => p.Id == id);
    }

    // Controller
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _service;

        public PokemonController(IPokemonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pokemon = await _service.GetByIdAsync(id);
            return pokemon == null ? NotFound() : Ok(pokemon);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Pokemon pokemon)
        {
            await _service.AddAsync(pokemon);
            return CreatedAtAction(nameof(GetById), new { id = pokemon.Id }, pokemon);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Pokemon pokemon)
        {
            await _service.UpdateAsync(pokemon);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    // Startup Configuration
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            // Load environment variables
            DotNetEnv.Env.Load();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Retrieve the MongoDB connection string from the .env file
            var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");

            // Load database settings
            var databaseSettings = _configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            databaseSettings.ConnectionString = connectionString;

            // Register services
            services.AddSingleton(databaseSettings);
            services.AddSingleton<IPokemonService, MongoDbService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    // Program Setup
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load environment variables
            DotNetEnv.Env.Load();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
