using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuAPIWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserIdentifier",
                table: "FavoriteDishes");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "FavoriteDishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteDishes_UserId",
                table: "FavoriteDishes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteDishes_Users_UserId",
                table: "FavoriteDishes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteDishes_Users_UserId",
                table: "FavoriteDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteDishes_UserId",
                table: "FavoriteDishes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FavoriteDishes");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserIdentifier",
                table: "FavoriteDishes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
