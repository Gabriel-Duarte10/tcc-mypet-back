using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcc_mypet_back.Migrations
{
    /// <inheritdoc />
    public partial class trazerImagemUserPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserImages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetId1",
                table: "PetImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_UserId1",
                table: "UserImages",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_PetImages_PetId1",
                table: "PetImages",
                column: "PetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PetImages_Pets_PetId1",
                table: "PetImages",
                column: "PetId1",
                principalTable: "Pets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserImages_Users_UserId1",
                table: "UserImages",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetImages_Pets_PetId1",
                table: "PetImages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_Users_UserId1",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_UserImages_UserId1",
                table: "UserImages");

            migrationBuilder.DropIndex(
                name: "IX_PetImages_PetId1",
                table: "PetImages");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "PetId1",
                table: "PetImages");
        }
    }
}
