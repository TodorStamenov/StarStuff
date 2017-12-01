namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SendMigrationColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendApplication",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendApplication",
                table: "AspNetUsers");
        }
    }
}