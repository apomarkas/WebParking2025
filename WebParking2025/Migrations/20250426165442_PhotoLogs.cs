using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebParking2025.Migrations
{
    /// <inheritdoc />
    public partial class PhotoLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "photo",
                table: "ReservationLogs",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photo",
                table: "ReservationLogs");
        }
    }
}
