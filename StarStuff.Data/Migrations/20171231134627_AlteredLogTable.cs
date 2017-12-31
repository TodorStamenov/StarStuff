namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AlteredLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogType",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Logs",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Logs",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Logs",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "TableName",
                table: "Logs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "LogType",
                table: "Logs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "Logs",
                nullable: true);
        }
    }
}