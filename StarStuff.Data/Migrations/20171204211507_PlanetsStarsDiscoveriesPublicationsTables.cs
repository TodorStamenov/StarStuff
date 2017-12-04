namespace StarStuff.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;

    public partial class PlanetsStarsDiscoveriesPublicationsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discoveries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateMade = table.Column<DateTime>(nullable: false),
                    StarSystem = table.Column<string>(maxLength: 255, nullable: false),
                    TelescopeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discoveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discoveries_Telescopes_TelescopeId",
                        column: x => x.TelescopeId,
                        principalTable: "Telescopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observers",
                columns: table => new
                {
                    ObserverId = table.Column<int>(nullable: false),
                    DiscoveryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observers", x => new { x.ObserverId, x.DiscoveryId });
                    table.ForeignKey(
                        name: "FK_Observers_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observers_AspNetUsers_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pioneers",
                columns: table => new
                {
                    PioneerId = table.Column<int>(nullable: false),
                    DiscoveryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pioneers", x => new { x.PioneerId, x.DiscoveryId });
                    table.ForeignKey(
                        name: "FK_Pioneers_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pioneers_AspNetUsers_PioneerId",
                        column: x => x.PioneerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscoveryId = table.Column<int>(nullable: false),
                    Mass = table.Column<double>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planets_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    DiscoveryId = table.Column<int>(nullable: false),
                    JournalId = table.Column<int>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Publications_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiscoveryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Temperature = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stars_Discoveries_DiscoveryId",
                        column: x => x.DiscoveryId,
                        principalTable: "Discoveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discoveries_TelescopeId",
                table: "Discoveries",
                column: "TelescopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Observers_DiscoveryId",
                table: "Observers",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pioneers_DiscoveryId",
                table: "Pioneers",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_DiscoveryId",
                table: "Planets",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_DiscoveryId",
                table: "Publications",
                column: "DiscoveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_JournalId",
                table: "Publications",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_Stars_DiscoveryId",
                table: "Stars",
                column: "DiscoveryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observers");

            migrationBuilder.DropTable(
                name: "Pioneers");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "Stars");

            migrationBuilder.DropTable(
                name: "Discoveries");
        }
    }
}