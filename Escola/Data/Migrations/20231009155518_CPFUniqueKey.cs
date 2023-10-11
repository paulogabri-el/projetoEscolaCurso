using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Escola.Data.Migrations
{
    public partial class CPFUniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Aluno_CPF",
                table: "Aluno",
                column: "CPF",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Aluno_CPF",
                table: "Aluno");
        }
    }
}
