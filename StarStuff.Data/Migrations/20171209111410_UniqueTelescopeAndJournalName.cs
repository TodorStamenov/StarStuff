namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UniqueTelescopeAndJournalName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Telescopes_Name",
                table: "Telescopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Journals_Name",
                table: "Journals",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Telescopes_Name",
                table: "Telescopes");

            migrationBuilder.DropIndex(
                name: "IX_Journals_Name",
                table: "Journals");
        }
    }
}