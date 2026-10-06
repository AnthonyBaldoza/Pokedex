using Pokedex.DTOs;
using Pokedex.Models;
using Pokedex.Repositories;

namespace Pokedex.Services
{
    // ============================================================
    // POKEMON SERVICE — Naglalaman ng BUSINESS LOGIC
    //
    // OOP CONCEPTS:
    // - ENCAPSULATION: _repository ay private
    // - ABSTRACTION: Itinatago ang database details mula sa Controller
    // - DEPENDENCY INJECTION: IPokemonRepository ay ibinibigay sa constructor
    //
    // CHANGES FROM ORIGINAL:
    // - MapToModel() ay gumagamit na ng PUBLIC CONSTRUCTOR ng Pokemon
    //   sa halip ng object initializer syntax { Name = ..., HP = ... }.
    //
    // BAKIT IMPORTANTE ITO?
    // — Ang dating object initializer ay nag-bypass ng encapsulation:
    //     new Pokemon { HP = -999 } ← walang validation!
    // — Ngayon, gumagamit ng constructor:
    //     new Pokemon(hp: value, ...) ← dadaan sa validated setter!
    // ============================================================
    public class PokemonService
    {
        private readonly IPokemonRepository _repository;

        public PokemonService(IPokemonRepository repository)
        {
            _repository = repository;
        }

        // GET ALL
        public async Task<IEnumerable<PokemonResponseDto>> GetAllAsync()
        {
            var pokemons = await _repository.GetAllAsync();
            return pokemons.Select(MapToResponseDto);
        }

            // GET BY ID
            public async Task<PokemonResponseDto?> GetByIdAsync(int id)
            {
                var pokemon = await _repository.GetByIdAsync(id);
                if (pokemon == null) return null;
                return MapToResponseDto(pokemon);
            }

        // SEARCH BY NAME
        public async Task<IEnumerable<PokemonResponseDto>> SearchByNameAsync(string name)
        {
            var pokemons = await _repository.SearchByNameAsync(name);
            return pokemons.Select(MapToResponseDto);
        }

        // FILTER BY TYPE
        public async Task<IEnumerable<PokemonResponseDto>> FilterByTypeAsync(string type)
        {
            var pokemons = await _repository.FilterByTypeAsync(type);
            return pokemons.Select(MapToResponseDto);
        }

        // CREATE
        public async Task<PokemonResponseDto> CreateAsync(PokemonRequestDto dto)
        {
            var pokemon = MapToModel(dto);
            var created = await _repository.CreateAsync(pokemon);
            return MapToResponseDto(created);
        }

        // UPDATE
        public async Task<PokemonResponseDto?> UpdateAsync(int id, PokemonRequestDto dto)
        {
            var pokemon = MapToModel(dto);
            var updated = await _repository.UpdateAsync(id, pokemon);
            if (updated == null) return null;
            return MapToResponseDto(updated);
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        // ============================================================
        // PRIVATE MAPPING METHODS
        // ============================================================

        // DTO → Model
        // UPDATED: Gumagamit na ng PUBLIC CONSTRUCTOR ng Pokemon
        // para masigurado na dumadaan sa validated setters ang lahat ng values!
        //
        // DATING CODE (hindi encapsulated):
        // return new Pokemon
        // {
        //     Name = dto.Name,   ← nag-bypass ng validation!
        //     HP = dto.HP,       ← nag-bypass ng validation!
        // };
        //
        // BAGONG CODE (encapsulated):
        // return new Pokemon(name: dto.Name, hp: dto.HP, ...)
        //   ← dumadaan sa constructor → setter → validated!
        private static Pokemon MapToModel(PokemonRequestDto dto)
        {
            return new Pokemon(
                name: dto.Name,
                type: dto.Type,
                description: dto.Description,
                hp: dto.HP,
                attack: dto.Attack,
                defense: dto.Defense,
                spAttack: dto.SpAttack,
                spDefense: dto.SpDefense,
                speed: dto.Speed,
                height: (float)dto.Height,
                weight: (float)dto.Weight,
                isLegendary: dto.IsLegendary,
                type2: dto.Type2,
                evolvesFromId: dto.EvolvesFromId,
                evolvesToId: dto.EvolvesToId
            );
        }

        // Model → ResponseDto (hindi nagbago)
        private static PokemonResponseDto MapToResponseDto(Pokemon pokemon)
        {
            return new PokemonResponseDto
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Type = pokemon.Type,
                Type2 = pokemon.Type2,
                Description = pokemon.Description,
                HP = pokemon.HP,
                Attack = pokemon.Attack,
                Defense = pokemon.Defense,
                SpAttack = pokemon.SpAttack,
                SpDefense = pokemon.SpDefense,
                Speed = pokemon.Speed,
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                IsLegendary = pokemon.IsLegendary,
                EvolvesFromId = pokemon.EvolvesFromId,
                EvolvesToId = pokemon.EvolvesToId
            };
        }
    }
}