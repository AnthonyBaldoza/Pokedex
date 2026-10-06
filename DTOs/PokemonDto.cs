using System.ComponentModel.DataAnnotations;

namespace Pokedex.DTOs
{
    // ============================================================
    // REQUEST DTO — Ito ang shape ng data na TINATANGGAP ng API
    // Ginagamit sa POST (Create) at PUT (Update)
    // Walang Id — hindi dapat i-set ng user ang Id
    // ============================================================
    public class PokemonRequestDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Primary type is required.")]
        public string Type { get; set; } = string.Empty;

        public string? Type2 { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(1, 255, ErrorMessage = "HP must be between 1 and 255.")]
        public int HP { get; set; }

        [Range(1, 255, ErrorMessage = "Attack must be between 1 and 255.")]
        public int Attack { get; set; }

        [Range(1, 255, ErrorMessage = "Defense must be between 1 and 255.")]
        public int Defense { get; set; }

        [Range(1, 255, ErrorMessage = "Sp. Attack must be between 1 and 255.")]
        public int SpAttack { get; set; }

        [Range(1, 255, ErrorMessage = "Sp. Defense must be between 1 and 255.")]
        public int SpDefense { get; set; }

        [Range(1, 255, ErrorMessage = "Speed must be between 1 and 255.")]
        public int Speed { get; set; }

        [Range(0.1, 20.0, ErrorMessage = "Height must be between 0.1m and 20m.")]
        public double Height { get; set; }

        [Range(0.1, 1000.0, ErrorMessage = "Weight must be between 0.1kg and 1000kg.")]
        public double Weight { get; set; }

        public bool IsLegendary { get; set; }

        public int? EvolvesFromId { get; set; }
        public int? EvolvesToId { get; set; }
    }

    // ============================================================
    // RESPONSE DTO — Ito ang shape ng data na IBINABALIK ng API
    // Ginagamit sa GET, POST response, PUT response
    // May Id na — kailangan ng client ang Id para sa edit/delete
    // ============================================================
    public class PokemonResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Type2 { get; set; }
        public string Description { get; set; } = string.Empty;
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
        public int Speed { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool IsLegendary { get; set; }
        public int? EvolvesFromId { get; set; }
        public int? EvolvesToId { get; set; }
    }
}