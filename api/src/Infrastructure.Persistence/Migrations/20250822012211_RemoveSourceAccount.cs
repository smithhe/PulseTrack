using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSourceAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViewFilters_Views_ViewId",
                table: "ViewFilters"
            );

            migrationBuilder.DropForeignKey(name: "FK_Views_Projects_ProjectId", table: "Views");

            migrationBuilder.DropTable(name: "SourceAccounts");

            migrationBuilder.DropIndex(name: "IX_Views_ProjectId", table: "Views");

            migrationBuilder.DropIndex(name: "IX_ViewFilters_ViewId", table: "ViewFilters");

            migrationBuilder.DropColumn(name: "SourceAccountId", table: "Projects");

            migrationBuilder.DropColumn(name: "SourceAccountId", table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SourceAccountId",
                table: "Projects",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "SourceAccountId",
                table: "Items",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "SourceAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    CredentialsJson = table.Column<string>(type: "text", nullable: false),
                    LastSyncAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceAccounts", x => x.Id);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Views_ProjectId",
                table: "Views",
                column: "ProjectId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ViewFilters_ViewId",
                table: "ViewFilters",
                column: "ViewId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ViewFilters_Views_ViewId",
                table: "ViewFilters",
                column: "ViewId",
                principalTable: "Views",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Views_Projects_ProjectId",
                table: "Views",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id"
            );
        }
    }
}
