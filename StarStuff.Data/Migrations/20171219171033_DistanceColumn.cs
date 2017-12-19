namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class DistanceColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Distance",
                table: "Discoveries",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Discoveries");
        }
    }
}