using System.ComponentModel.DataAnnotations;

namespace Pokedex.Models
{
    // ============================================================
    // POKEMON — Properly Encapsulated Entity/Model
    //
    // OOP CONCEPTS APPLIED:
    // ✅ ENCAPSULATION  — private backing fields, validated setters
    // ✅ ABSTRACTION    — internal state hidden, only behavior exposed
    // ✅ CONSTRUCTOR    — tanging paraan para gumawa ng valid Pokemon
    //
    // BAKIT BINAGO?
    // — Ang dating version ay walang protection.
    //   Kahit invalid na value, tinatanggap pa rin.
    // — Ngayon, ang bawat property ay may validation sa loob ng setter.
    //   Hindi na pwedeng mag-set ng HP = -999 o Name = ""
    //   nang walang exception!
    //
    // EF CORE COMPATIBILITY:
    // — Ginagamit ang 'protected Pokemon()' (parameterless constructor)
    //   para makapag-map ang EF Core ng values mula sa database.
    // — Protected = accessible ng EF Core internally, pero
    //   hindi accessible mula sa labas ng class hierarchy.
    // ============================================================
    public class Pokemon
    {
        // ============================================================
        // PRIVATE BACKING FIELDS
        // Dito talaga nakalagay ang mga values — NAKATAGO SA LABAS!
        // Hindi pwedeng direktang ma-access ng kahit sinong code
        // sa labas ng class na ito.
        // ============================================================
        private string _name = string.Empty;
        private string _type = string.Empty;
        private string? _type2;
        private string _description = string.Empty;
        private int _hp;
        private int _attack;
        private int _defense;
        private int _spAttack;
        private int _spDefense;
        private int _speed;
        private float _height;
        private float _weight;

        // ============================================================
        // PROTECTED PARAMETERLESS CONSTRUCTOR — Para sa EF Core
        //
        // Bakit PROTECTED at hindi PRIVATE?
        // — EF Core internally ay gumagawa ng instance ng Pokemon
        //   kapag nag-query ng database. Kailangan niya ng
        //   parameterless constructor para magawa ito.
        // — PROTECTED = accessible ng EF Core at ng subclasses,
        //   pero HINDI accessible ng external code.
        //   Ibig sabihin: hindi pwedeng gawin ng external code:
        //   var p = new Pokemon(); ← COMPILE ERROR!
        //   Kailangan dumaan sa public constructor.
        // ============================================================
        protected Pokemon() { }

        // ============================================================
        // PUBLIC CONSTRUCTOR — Ang TANGING paraan para gumawa ng
        // VALID Pokemon object mula sa labas ng class.
        //
        // Lahat ng required fields ay kailangan ibigay dito.
        // Hindi makakalusot ang invalid data kasi dumadaan
        // sa validated properties (HP = hp ay tatawag sa setter).
        // ============================================================
        public Pokemon(
            string name,
            string type,
            string description,
            int hp,
            int attack,
            int defense,
            int spAttack,
            int spDefense,
            int speed,
            float height,
            float weight,
            bool isLegendary = false,
            string? type2 = null,
            int? evolvesFromId = null,
            int? evolvesToId = null)
        {
            // Ang bawat assignment ay tatawag sa validated SETTER
            // Hindi direktang pumupunta sa private field!
            Name = name;
            Type = type;
            Type2 = type2;
            Description = description;
            HP = hp;
            Attack = attack;
            Defense = defense;
            SpAttack = spAttack;
            SpDefense = spDefense;
            Speed = speed;
            Height = height;
            Weight = weight;
            IsLegendary = isLegendary;
            EvolvesFromId = evolvesFromId;
            EvolvesToId = evolvesToId;
        }

        // ============================================================
        // ID — Read-Only mula sa labas
        // Ang database (EF Core) ang mag-se-set nito via private set.
        // Walang external code ang pwedeng mag-set ng Id directly.
        // ============================================================
        public int Id { get; private set; }

        // ============================================================
        // PROPERTIES na may VALIDATION sa SETTER
        //
        // Pattern:
        // public TYPE PropertyName
        // {
        //     get { return _backingField; }  ← Ibigay ang value
        //     set
        //     {
        //         [validation logic]         ← Suriin muna!
        //         _backingField = value;     ← Itago sa private field
        //     }
        // }
        // ============================================================

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name is required.");
                if (value.Length > 100)
                    throw new ArgumentException("Name must not exceed 100 characters.");
                _name = value.Trim();
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Primary type is required.");
                if (value.Length > 50)
                    throw new ArgumentException("Type must not exceed 50 characters.");
                _type = value.Trim();
            }
        }

        public string? Type2
        {
            get { return _type2; }
            set
            {
                // Nullable — pwedeng null (hindi lahat ng Pokemon ay dual-type)
                if (value != null && value.Length > 50)
                    throw new ArgumentException("Type2 must not exceed 50 characters.");
                _type2 = value?.Trim();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description is required.");
                if (value.Length < 5 || value.Length > 500)
                    throw new ArgumentException("Description must be between 5 and 500 characters.");
                _description = value.Trim();
            }
        }

        public int HP
        {
            get { return _hp; }
            set
            {
                if (value < 1 || value > 255)
                    throw new ArgumentException("HP must be between 1 and 255.");
                _hp = value;
            }
        }

        public int Attack
        {
            get { return _attack; }
            set
            {
                if (value < 1 || value > 255)
                    throw new ArgumentException("Attack must be between 1 and 255.");
                _attack = value;
            }
        }

        public int Defense
        {
            get { return _defense; }
            set
            {
                if (value < 1 || value > 255)
                    throw new ArgumentException("Defense must be between 1 and 255.");
                _defense = value;
            }
        }

        public int SpAttack
        {
            get { return _spAttack; }
            set
            {
                if (value < 1 || value > 255)
                    throw new ArgumentException("Sp. Attack must be between 1 and 255.");
                _spAttack = value;
            }
        }

        public int SpDefense
        {
            get { return _spDefense; }
            set
            {
                if (value < 1 || value > 255)
                    throw new ArgumentException("Sp. Defense must be between 1 and 255.");
                _spDefense = value;
            }
        }

        public int Speed
        {
            get { return _speed; }
            set
            {
                if (value < 1 || value > 255)
                    throw new ArgumentException("Speed must be between 1 and 255.");
                _speed = value;
            }
        }

        public float Height
        {
            get { return _height; }
            set
            {
                if (value < 0.1f || value > 20.0f)
                    throw new ArgumentException("Height must be between 0.1m and 20m.");
                _height = value;
            }
        }

        public float Weight
        {
            get { return _weight; }
            set
            {
                if (value < 0.1f || value > 1000.0f)
                    throw new ArgumentException("Weight must be between 0.1kg and 1000kg.");
                _weight = value;
            }
        }

        // Simple properties — walang complex validation needed
        public bool IsLegendary { get; set; }
        public int? EvolvesFromId { get; set; }
        public int? EvolvesToId { get; set; }

        // ============================================================
        // COMPUTED / READ-ONLY PROPERTY — Walang setter!
        // Automatically calculated base sa ibang properties.
        // Hindi pwedeng i-set ito ng external code.
        // Ito ay isang BONUS na nagpapakita ng proper encapsulation!
        // ============================================================
        public int TotalStats
        {
            get { return HP + Attack + Defense + SpAttack + SpDefense + Speed; }
        }

        // ============================================================
        // UPDATE METHOD — Para sa clean na pag-update ng Pokemon
        //
        // Sa halip na direktang i-set ang fields mula sa labas:
        //   existing.Name = "NewName"; ← old way, walang protection
        //
        // Ngayon, may dedicated method:
        //   existing.Update("NewName", ...); ← dumadaan sa validation!
        //
        // Ginagamit ito ng PokemonRepository sa UpdateAsync()
        // ============================================================
        public void Update(
            string name,
            string type,
            string description,
            int hp,
            int attack,
            int defense,
            int spAttack,
            int spDefense,
            int speed,
            float height,
            float weight,
            bool isLegendary,
            string? type2 = null,
            int? evolvesFromId = null,
            int? evolvesToId = null)
        {
            // Lahat ay dadaan sa validated setters!
            Name = name;
            Type = type;
            Type2 = type2;
            Description = description;
            HP = hp;
            Attack = attack;
            Defense = defense;
            SpAttack = spAttack;
            SpDefense = spDefense;
            Speed = speed;
            Height = height;
            Weight = weight;
            IsLegendary = isLegendary;
            EvolvesFromId = evolvesFromId;
            EvolvesToId = evolvesToId;
        }
    }
}