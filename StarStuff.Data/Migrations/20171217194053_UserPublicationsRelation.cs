namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UserPublicationsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Publications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Publications_AuthorId",
                table: "Publications",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_AuthorId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Publications");
        }
    }
}