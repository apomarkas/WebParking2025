using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebParking2025.Migrations
{
    /// <inheritdoc />
    public partial class ReservationLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationLogs",
                columns: table => new
                {
                    log_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationLogs", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_ReservationLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReservationLogs_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "reservation_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationLogs_ReservationId",
                table: "ReservationLogs",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationLogs_UserId",
                table: "ReservationLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationLogs");
        }
    }
}
