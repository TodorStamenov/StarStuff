namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class StarSystemPlanetStarColumnIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stars_Name",
                table: "Stars",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Planets_Name",
                table: "Planets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discoveries_StarSystem",
                table: "Discoveries",
                column: "StarSystem",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stars_Name",
                table: "Stars");

            migrationBuilder.DropIndex(
                name: "IX_Planets_Name",
                table: "Planets");

            migrationBuilder.DropIndex(
                name: "IX_Discoveries_StarSystem",
                table: "Discoveries");
        }
    }
}