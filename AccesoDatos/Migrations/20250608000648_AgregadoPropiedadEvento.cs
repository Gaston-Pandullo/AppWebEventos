using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoPropiedadEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ubicacion",
                table: "Eventos",
                newName: "UbicacionGoogleMaps");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Eventos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "UbicacionGoogleMaps",
                table: "Eventos",
                newName: "Ubicacion");
        }
    }
}
