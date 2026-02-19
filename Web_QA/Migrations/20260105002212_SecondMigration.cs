using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_QA.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponsableId",
                table: "Projets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projets_ResponsableId",
                table: "Projets",
                column: "ResponsableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projets_Users_ResponsableId",
                table: "Projets",
                column: "ResponsableId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projets_Users_ResponsableId",
                table: "Projets");

            migrationBuilder.DropIndex(
                name: "IX_Projets_ResponsableId",
                table: "Projets");

            migrationBuilder.DropColumn(
                name: "ResponsableId",
                table: "Projets");
        }
    }
}
