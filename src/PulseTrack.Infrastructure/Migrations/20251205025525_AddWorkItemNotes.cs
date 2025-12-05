using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkItemNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "WorkItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "WorkItems");
        }
    }
}
