using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoLibrary.Data.Migrations
{
    public partial class NavigationPropFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_AspNetUsers_UserId1",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_UserId1",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Pictures");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pictures",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UserId",
                table: "Pictures",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_AspNetUsers_UserId",
                table: "Pictures",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_AspNetUsers_UserId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_UserId",
                table: "Pictures");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Pictures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UserId1",
                table: "Pictures",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_AspNetUsers_UserId1",
                table: "Pictures",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
