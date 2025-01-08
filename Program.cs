using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


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

    // Implementation of Pokemon Service
    public class PokemonService : IPokemonService
    {
        private readonly List<Pokemon> _pokemonList = new();

        public Task<List<Pokemon>> GetAllAsync() => Task.FromResult(_pokemonList);

        public Task<Pokemon> GetByIdAsync(int id)
        {
            var pokemon = _pokemonList.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(pokemon);
        }

        public Task AddAsync(Pokemon pokemon)
        {
            _pokemonList.Add(pokemon);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Pokemon pokemon)
        {
            var existing = _pokemonList.FirstOrDefault(p => p.Id == pokemon.Id);
            if (existing != null)
            {
                existing.Name = pokemon.Name;
                existing.Type = pokemon.Type;
                existing.Ability = pokemon.Ability;
                existing.Level = pokemon.Level;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var pokemon = _pokemonList.FirstOrDefault(p => p.Id == id);
            if (pokemon != null)
                _pokemonList.Remove(pokemon);
            return Task.CompletedTask;
        }
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

    // Program Setup
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IPokemonService, PokemonService>();
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

    public class Program
    {
        public static void Main(string[] args)
        {
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
