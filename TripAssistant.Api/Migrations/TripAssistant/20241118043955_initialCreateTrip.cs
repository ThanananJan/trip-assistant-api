using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripAssistant.Api.Migrations.TripAssistant
{
    /// <inheritdoc />
    public partial class initialCreateTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    IdTrip = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NamTrip = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DtmCreate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.IdTrip);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TripMember",
                columns: table => new
                {
                    IdTripMember = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTrip = table.Column<int>(type: "int", nullable: false),
                    NamMember = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripMember", x => x.IdTripMember);
                    table.ForeignKey(
                        name: "FK_TripMember_Trip_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trip",
                        principalColumn: "IdTrip",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TripUser",
                columns: table => new
                {
                    IdTrip = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripUser", x => new { x.IdTrip, x.IdUser });
                    table.ForeignKey(
                        name: "FK_TripUser_Trip_IdTrip",
                        column: x => x.IdTrip,
                        principalTable: "Trip",
                        principalColumn: "IdTrip",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TripTransaction",
                columns: table => new
                {
                    IdTripTransaction = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTrip = table.Column<int>(type: "int", nullable: false),
                    DscTransaction = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    DtmCreate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IdPayer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTransaction", x => x.IdTripTransaction);
                    table.ForeignKey(
                        name: "FK_TripTransaction_TripMember_IdPayer",
                        column: x => x.IdPayer,
                        principalTable: "TripMember",
                        principalColumn: "IdTripMember",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TripDebtor",
                columns: table => new
                {
                    IdTripTransaction = table.Column<int>(type: "int", nullable: false),
                    IdDebtor = table.Column<int>(type: "int", nullable: false),
                    TripTransactionIdTripTransaction = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripDebtor", x => new { x.IdTripTransaction, x.IdDebtor });
                    table.ForeignKey(
                        name: "FK_TripDebtor_TripMember_IdDebtor",
                        column: x => x.IdDebtor,
                        principalTable: "TripMember",
                        principalColumn: "IdTripMember",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripDebtor_TripTransaction_TripTransactionIdTripTransaction",
                        column: x => x.TripTransactionIdTripTransaction,
                        principalTable: "TripTransaction",
                        principalColumn: "IdTripTransaction");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TripDebtor_IdDebtor",
                table: "TripDebtor",
                column: "IdDebtor");

            migrationBuilder.CreateIndex(
                name: "IX_TripDebtor_TripTransactionIdTripTransaction",
                table: "TripDebtor",
                column: "TripTransactionIdTripTransaction");

            migrationBuilder.CreateIndex(
                name: "IX_TripMember_IdTrip",
                table: "TripMember",
                column: "IdTrip");

            migrationBuilder.CreateIndex(
                name: "IX_TripTransaction_IdPayer",
                table: "TripTransaction",
                column: "IdPayer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripDebtor");

            migrationBuilder.DropTable(
                name: "TripUser");

            migrationBuilder.DropTable(
                name: "TripTransaction");

            migrationBuilder.DropTable(
                name: "TripMember");

            migrationBuilder.DropTable(
                name: "Trip");
        }
    }
}
