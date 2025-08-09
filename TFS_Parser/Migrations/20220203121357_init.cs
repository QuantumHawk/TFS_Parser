using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TFS_Parser.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TFSes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ANCESTORLIST = table.Column<string>(type: "text", nullable: true),
                    OGRSOVMLIST = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TFSes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROOTALTERNATELISTALT",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Num = table.Column<string>(type: "text", nullable: true),
                    NodeStartID = table.Column<string>(type: "text", nullable: true),
                    NodeEndID = table.Column<string>(type: "text", nullable: true),
                    TFSID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOTALTERNATELISTALT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROOTALTERNATELISTALT_TFSes_TFSID",
                        column: x => x.TFSID,
                        principalTable: "TFSes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROOTMAINLISTTFS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TypeID = table.Column<string>(type: "text", nullable: true),
                    StartPointX = table.Column<string>(type: "text", nullable: true),
                    StartPointY = table.Column<string>(type: "text", nullable: true),
                    OffsetX = table.Column<string>(type: "text", nullable: true),
                    OffsetY = table.Column<string>(type: "text", nullable: true),
                    NextID = table.Column<string>(type: "text", nullable: true),
                    PriorID = table.Column<string>(type: "text", nullable: true),
                    TFSID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOTMAINLISTTFS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROOTMAINLISTTFS_TFSes_TFSID",
                        column: x => x.TFSID,
                        principalTable: "TFSes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROOTTYPEDECISION",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Sovm = table.Column<string>(type: "text", nullable: true),
                    TFSID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOTTYPEDECISION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROOTTYPEDECISION_TFSes_TFSID",
                        column: x => x.TFSID,
                        principalTable: "TFSes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROOTTYPEPARAM",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    TFSID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOTTYPEPARAM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROOTTYPEPARAM_TFSes_TFSID",
                        column: x => x.TFSID,
                        principalTable: "TFSes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROOTALTERNATELISTALTITEM",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ROOTALTERNATELISTALTID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOTALTERNATELISTALTITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROOTALTERNATELISTALTITEM_ROOTALTERNATELISTALT_ROOTALTERNATE~",
                        column: x => x.ROOTALTERNATELISTALTID,
                        principalTable: "ROOTALTERNATELISTALT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROOTMAINLISTTFSTFE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    TypeID = table.Column<string>(type: "text", nullable: true),
                    ROOTMAINLISTTFSID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOTMAINLISTTFSTFE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROOTMAINLISTTFSTFE_ROOTMAINLISTTFS_ROOTMAINLISTTFSID",
                        column: x => x.ROOTMAINLISTTFSID,
                        principalTable: "ROOTMAINLISTTFS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROOTTYPEDECISIONParams",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    V = table.Column<string>(type: "text", nullable: true),
                    T = table.Column<string>(type: "text", nullable: true),
                    VD = table.Column<string>(type: "text", nullable: true),
                    TD = table.Column<string>(type: "text", nullable: true),
                    ROOTTYPEDECISIONID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOTTYPEDECISIONParams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROOTTYPEDECISIONParams_ROOTTYPEDECISION_ROOTTYPEDECISIONID",
                        column: x => x.ROOTTYPEDECISIONID,
                        principalTable: "ROOTTYPEDECISION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ROOTALTERNATELISTALT_TFSID",
                table: "ROOTALTERNATELISTALT",
                column: "TFSID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOTALTERNATELISTALTITEM_ROOTALTERNATELISTALTID",
                table: "ROOTALTERNATELISTALTITEM",
                column: "ROOTALTERNATELISTALTID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOTMAINLISTTFS_TFSID",
                table: "ROOTMAINLISTTFS",
                column: "TFSID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOTMAINLISTTFSTFE_ROOTMAINLISTTFSID",
                table: "ROOTMAINLISTTFSTFE",
                column: "ROOTMAINLISTTFSID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOTTYPEDECISION_TFSID",
                table: "ROOTTYPEDECISION",
                column: "TFSID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOTTYPEDECISIONParams_ROOTTYPEDECISIONID",
                table: "ROOTTYPEDECISIONParams",
                column: "ROOTTYPEDECISIONID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOTTYPEPARAM_TFSID",
                table: "ROOTTYPEPARAM",
                column: "TFSID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROOTALTERNATELISTALTITEM");

            migrationBuilder.DropTable(
                name: "ROOTMAINLISTTFSTFE");

            migrationBuilder.DropTable(
                name: "ROOTTYPEDECISIONParams");

            migrationBuilder.DropTable(
                name: "ROOTTYPEPARAM");

            migrationBuilder.DropTable(
                name: "ROOTALTERNATELISTALT");

            migrationBuilder.DropTable(
                name: "ROOTMAINLISTTFS");

            migrationBuilder.DropTable(
                name: "ROOTTYPEDECISION");

            migrationBuilder.DropTable(
                name: "TFSes");
        }
    }
}
