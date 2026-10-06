using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pokedex.Migrations
{
    /// <inheritdoc />
    public partial class AddStatsAndEvolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attack",
                table: "Pokemons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Defense",
                table: "Pokemons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvolvesFromId",
                table: "Pokemons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvolvesToId",
                table: "Pokemons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HP",
                table: "Pokemons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsLegendary",
                table: "Pokemons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SpAttack",
                table: "Pokemons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpDefense",
                table: "Pokemons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Pokemons",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attack",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Defense",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "EvolvesFromId",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "EvolvesToId",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "HP",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "IsLegendary",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "SpAttack",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "SpDefense",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Pokemons");
        }
    }
}
