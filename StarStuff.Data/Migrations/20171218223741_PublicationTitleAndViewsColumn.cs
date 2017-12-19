namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class PublicationTitleAndViewsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Publications",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Publications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Publications_Title",
                table: "Publications",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Publications_Title",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Publications");
        }
    }
}