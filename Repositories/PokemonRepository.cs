using Microsoft.EntityFrameworkCore;
using Pokedex.Data;
using Pokedex.Models;

namespace Pokedex.Repositories
{
    // ============================================================
    // REPOSITORY — Naglalaman ng LAHAT ng database operations
    // Implements IPokemonRepository interface
    //
    // OOP CONCEPTS:
    // - IMPLEMENTS interface (contract)
    // - ENCAPSULATION: _context ay private, hindi accessible sa labas
    // - DEPENDENCY INJECTION: DbContext ay ibinibigay sa constructor
    //
    // CHANGES FROM ORIGINAL:
    // - UpdateAsync() ay hindi na direktang nag-se-set ng fields.
    //   Gumagamit na ng existing.Update(...) method ng Pokemon class.
    //   Ito ay nagre-respeto sa encapsulation ng Pokemon model.
    // - CreateAsync() ay hindi na gumagawa ng 'new Pokemon { ... }'
    //   sa loob — ginagawa na iyon ng Service gamit ang constructor.
    // ============================================================
    public class PokemonRepository : IPokemonRepository
    {
        private readonly PokedexDbContext _context;

        // Constructor Injection — Dependency Injection
        public PokemonRepository(PokedexDbContext context)
        {
            _context = context;
        }

        // READ ALL
        public async Task<IEnumerable<Pokemon>> GetAllAsync()
        {
            return await _context.Pokemons
                .OrderBy(p => p.Id)
                .ToListAsync();
        }

        // READ ONE
        public async Task<Pokemon?> GetByIdAsync(int id)
        {
            return await _context.Pokemons.FindAsync(id);
        }

        // SEARCH — Case-insensitive search by name
        public async Task<IEnumerable<Pokemon>> SearchByNameAsync(string name)
        {
            return await _context.Pokemons
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        // FILTER — By primary or secondary type
        public async Task<IEnumerable<Pokemon>> FilterByTypeAsync(string type)
        {
            return await _context.Pokemons
                .Where(p => p.Type.ToLower() == type.ToLower() ||
                            (p.Type2 != null && p.Type2.ToLower() == type.ToLower()))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        // CREATE — Ang Pokemon object ay ginawa na ng Service
        // gamit ang proper constructor. Dito, i-save na lang sa DB.
        public async Task<Pokemon> CreateAsync(Pokemon pokemon)
        {
            _context.Pokemons.Add(pokemon);
            await _context.SaveChangesAsync();
            return pokemon;
        }

        // UPDATE — UPDATED!
        // Hindi na direktang nag-se-set ng fields.
        // Gumagamit na ng existing.Update() method ng Pokemon class.
        //
        // BAKIT ITO ANG TAMANG PARAAN?
        // — Ang Update() method ng Pokemon ay nag-va-validate ng data
        //   sa loob ng Pokemon class mismo.
        // — Nagre-respeto tayo sa encapsulation ng Pokemon.
        // — Ang Repository ay hindi na kailangang malaman
        //   kung anong validation ang kailangan — alam na ng Pokemon.
        public async Task<Pokemon?> UpdateAsync(int id, Pokemon updated)
        {
            var existing = await _context.Pokemons.FindAsync(id);
            if (existing == null) return null;

            // DATING CODE (hindi encapsulated):
            // existing.Name = updated.Name;
            // existing.HP = updated.HP;
            // existing.Attack = updated.Attack;
            // ... (direct field access — walang validation!)

            // BAGONG CODE (encapsulated — dumadaan sa Update method):
            existing.Update(
                name: updated.Name,
                type: updated.Type,
                description: updated.Description,
                hp: updated.HP,
                attack: updated.Attack,
                defense: updated.Defense,
                spAttack: updated.SpAttack,
                spDefense: updated.SpDefense,
                speed: updated.Speed,
                height: updated.Height,
                weight: updated.Weight,
                isLegendary: updated.IsLegendary,
                type2: updated.Type2,
                evolvesFromId: updated.EvolvesFromId,
                evolvesToId: updated.EvolvesToId
            );

            await _context.SaveChangesAsync();
            return existing;
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Pokemons.FindAsync(id);
            if (existing != null)
            {
                _context.Pokemons.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}