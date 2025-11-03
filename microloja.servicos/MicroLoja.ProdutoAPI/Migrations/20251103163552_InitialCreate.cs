using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroLoja.ProdutoAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icone",
                table: "categorias",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "categorias",
                keyColumn: "id",
                keyValue: 1,
                column: "icone",
                value: null);

            migrationBuilder.UpdateData(
                table: "categorias",
                keyColumn: "id",
                keyValue: 2,
                column: "icone",
                value: null);

            migrationBuilder.UpdateData(
                table: "categorias",
                keyColumn: "id",
                keyValue: 3,
                column: "icone",
                value: null);

            migrationBuilder.UpdateData(
                table: "categorias",
                keyColumn: "id",
                keyValue: 4,
                column: "icone",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icone",
                table: "categorias");
        }
    }
}
