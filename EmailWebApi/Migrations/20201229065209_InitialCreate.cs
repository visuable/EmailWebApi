using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailWebApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Bodies",
                table => new
                {
                    Id = table.Column<int>("int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>("nvarchar(max)", nullable: true),
                    Save = table.Column<bool>("bit")
                },
                constraints: table => { table.PrimaryKey("PK_Bodies", x => x.Id); });

            migrationBuilder.CreateTable(
                "Infos",
                table => new
                {
                    Id = table.Column<int>("int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniversalId = table.Column<Guid>("uniqueidentifier"),
                    Date = table.Column<DateTime>("datetime2")
                },
                constraints: table => { table.PrimaryKey("PK_Infos", x => x.Id); });

            migrationBuilder.CreateTable(
                "States",
                table => new
                {
                    Id = table.Column<int>("int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>("int")
                },
                constraints: table => { table.PrimaryKey("PK_States", x => x.Id); });

            migrationBuilder.CreateTable(
                "ThrottlingStates",
                table => new
                {
                    Id = table.Column<int>("int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Counter = table.Column<int>("int"),
                    LastAddressCounter = table.Column<int>("int"),
                    LastAddress = table.Column<string>("nvarchar(max)", nullable: true),
                    EndPoint = table.Column<DateTime>("datetime2")
                },
                constraints: table => { table.PrimaryKey("PK_ThrottlingStates", x => x.Id); });

            migrationBuilder.CreateTable(
                "Contents",
                table => new
                {
                    Id = table.Column<int>("int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>("nvarchar(max)", nullable: true),
                    BodyId = table.Column<int>("int", nullable: true),
                    Title = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        "FK_Contents_Bodies_BodyId",
                        x => x.BodyId,
                        "Bodies",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Emails",
                table => new
                {
                    Id = table.Column<int>("int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>("int", nullable: true),
                    InfoId = table.Column<int>("int", nullable: true),
                    StateId = table.Column<int>("int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                    table.ForeignKey(
                        "FK_Emails_Contents_ContentId",
                        x => x.ContentId,
                        "Contents",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Emails_Infos_InfoId",
                        x => x.InfoId,
                        "Infos",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Emails_States_StateId",
                        x => x.StateId,
                        "States",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                "ThrottlingStates",
                new[] {"Id", "Counter", "EndPoint", "LastAddress", "LastAddressCounter"},
                new object[]
                    {1, 0, new DateTime(2020, 12, 29, 9, 53, 7, 628, DateTimeKind.Local).AddTicks(852), "", 0});

            migrationBuilder.CreateIndex(
                "IX_Contents_BodyId",
                "Contents",
                "BodyId");

            migrationBuilder.CreateIndex(
                "IX_Emails_ContentId",
                "Emails",
                "ContentId");

            migrationBuilder.CreateIndex(
                "IX_Emails_InfoId",
                "Emails",
                "InfoId");

            migrationBuilder.CreateIndex(
                "IX_Emails_StateId",
                "Emails",
                "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Emails");

            migrationBuilder.DropTable(
                "ThrottlingStates");

            migrationBuilder.DropTable(
                "Contents");

            migrationBuilder.DropTable(
                "Infos");

            migrationBuilder.DropTable(
                "States");

            migrationBuilder.DropTable(
                "Bodies");
        }
    }
}