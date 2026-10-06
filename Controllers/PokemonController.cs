using Microsoft.AspNetCore.Mvc;
using Pokedex.DTOs;
using Pokedex.Services;

namespace Pokedex.Controllers
{
    // ============================================================
    // POKEMON CONTROLLER — Nag-ha-handle ng HTTP Requests/Responses
    //
    // OOP CONCEPTS:
    // - INHERITANCE: PokemonController : ControllerBase
    //   Nagmamana ng Ok(), NotFound(), BadRequest(), etc.
    // - DEPENDENCY INJECTION: PokemonService ay ibinibigay sa constructor
    // - ENCAPSULATION: _service ay private — hindi accessible sa labas
    //
    // RESPONSIBILITIES:
    // - Tanggapin ang HTTP requests
    // - I-validate ang input (automatic via [ApiController])
    // - Tawagin ang Service
    // - I-return ang tamang HTTP response at status code
    //
    // HINDI responsibilidad ng Controller:
    // - Database operations (Repository ang bahala)
    // - Business logic (Service ang bahala)
    // ============================================================
    [ApiController]
    [Route("api/pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonService _service;

        // Constructor — Dependency Injection
        // Ang .NET DI container ang magbibigay ng PokemonService instance
        public PokemonController(PokemonService service)
        {
            _service = service;
        }

        // ========================
        // GET api/pokemon
        // Kukunin ang LAHAT ng Pokemon
        // Response: 200 OK + List<PokemonResponseDto>
        // ========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonResponseDto>>> GetAll()
        {
            var pokemons = await _service.GetAllAsync();
            return Ok(pokemons);
        }

        // ========================
        // GET api/pokemon/{id}
        // Kukunin ang ISANG Pokemon base sa Id
        // Response: 200 OK + PokemonResponseDto
        //           404 Not Found kung wala
        // ========================
        [HttpGet("{id}")]
        public async Task<ActionResult<PokemonResponseDto>> GetById(int id)
        {
            var pokemon = await _service.GetByIdAsync(id);
            if (pokemon == null) return NotFound(new { message = $"Pokemon with ID {id} not found." });
            return Ok(pokemon);
        }

        // ========================
        // GET api/pokemon/search?name=Pikachu
        // Maghahanap ng Pokemon base sa name
        // Response: 200 OK + List<PokemonResponseDto>
        // ========================
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PokemonResponseDto>>> SearchByName(
            [FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search term cannot be empty." });

            var results = await _service.SearchByNameAsync(name);
            return Ok(results);
        }

        // ========================
        // GET api/pokemon/filter?type=Fire
        // Mag-filter ng Pokemon base sa type
        // Response: 200 OK + List<PokemonResponseDto>
        // ========================
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<PokemonResponseDto>>> FilterByType(
            [FromQuery] string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return BadRequest(new { message = "Type cannot be empty." });

            var results = await _service.FilterByTypeAsync(type);
            return Ok(results);
        }

        // ========================
        // POST api/pokemon
        // Gumawa ng BAGONG Pokemon
        // Request Body: PokemonRequestDto (JSON)
        // Response: 201 Created + PokemonResponseDto
        //           400 Bad Request kung invalid ang data
        // ========================
        [HttpPost]
        public async Task<ActionResult<PokemonResponseDto>> Create(
            [FromBody] PokemonRequestDto dto)
        {
            // Automatic validation ng [ApiController] + Data Annotations
            // Kung hindi valid ang dto, auto-return ng 400 Bad Request

            var created = await _service.CreateAsync(dto);

            // 201 Created — kasama ang Location header na nagtuturo sa bagong resource
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // ========================
        // PUT api/pokemon/{id}
        // I-update ang existing Pokemon
        // Request Body: PokemonRequestDto (JSON)
        // Response: 200 OK + updated PokemonResponseDto
        //           404 Not Found kung wala ang Id
        // ========================
        [HttpPut("{id}")]
        public async Task<ActionResult<PokemonResponseDto>> Update(
            int id, [FromBody] PokemonRequestDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound(new { message = $"Pokemon with ID {id} not found." });
            return Ok(updated);
        }

        // ========================
        // DELETE api/pokemon/{id}
        // Tanggalin ang Pokemon
        // Response: 204 No Content (walang body — normal para sa DELETE)
        //           Hindi nagre-return ng 404 — common practice
        // ========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent(); // 204
        }
    }
}