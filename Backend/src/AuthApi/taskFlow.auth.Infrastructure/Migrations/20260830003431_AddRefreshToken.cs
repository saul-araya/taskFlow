using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace taskFlow.auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProviders_Users_user_id",
                schema: "auth",
                table: "UserProviders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProviders",
                schema: "auth",
                table: "UserProviders");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "auth",
                newName: "user",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "UserProviders",
                schema: "auth",
                newName: "user_provider",
                newSchema: "auth");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                schema: "auth",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_provider",
                schema: "auth",
                table: "user_provider",
                column: "id");

            migrationBuilder.CreateTable(
                name: "refresh_token",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_REFRSH_TOKEN_USER",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REFRESH_TOKEN_ACTIVE",
                schema: "auth",
                table: "refresh_token",
                column: "refresh_token_hash",
                unique: true,
                filter: "\"is_active\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_user_id",
                schema: "auth",
                table: "refresh_token",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_provider_user_user_id",
                schema: "auth",
                table: "user_provider",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_provider_user_user_id",
                schema: "auth",
                table: "user_provider");

            migrationBuilder.DropTable(
                name: "refresh_token",
                schema: "auth");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_provider",
                schema: "auth",
                table: "user_provider");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                schema: "auth",
                table: "user");

            migrationBuilder.RenameTable(
                name: "user_provider",
                schema: "auth",
                newName: "UserProviders",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "user",
                schema: "auth",
                newName: "Users",
                newSchema: "auth");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProviders",
                schema: "auth",
                table: "UserProviders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "auth",
                table: "Users",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProviders_Users_user_id",
                schema: "auth",
                table: "UserProviders",
                column: "user_id",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
