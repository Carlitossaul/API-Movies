using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class fixPropertyDateCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Movie",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreateData",
                table: "Category",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Movie",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Category",
                newName: "CreateData");
        }
    }
}
