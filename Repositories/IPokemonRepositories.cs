using Pokedex.Models;

namespace Pokedex.Repositories
{
    // ============================================================
    // INTERFACE — Ito ang CONTRACT ng Repository
    // Sinasabi nito kung ANONG mga operations ang kailangan
    // Hindi ito naglalaman ng actual na code — abstract lang
    //
    // OOP CONCEPT: INTERFACE + ABSTRACTION
    // Ang PokemonService ay hindi nalalaman kung PAANO
    // nagtatrabaho ang database — basta may interface na susunodin
    // ============================================================
    public interface IPokemonRepository
    {
        Task<IEnumerable<Pokemon>> GetAllAsync();
        Task<Pokemon?> GetByIdAsync(int id);
        Task<IEnumerable<Pokemon>> SearchByNameAsync(string name);
        Task<IEnumerable<Pokemon>> FilterByTypeAsync(string type);
        Task<Pokemon> CreateAsync(Pokemon pokemon);
        Task<Pokemon?> UpdateAsync(int id, Pokemon pokemon);
        Task DeleteAsync(int id);
    }
}