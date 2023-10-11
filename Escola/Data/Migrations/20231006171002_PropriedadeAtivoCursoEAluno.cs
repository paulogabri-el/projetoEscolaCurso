using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Escola.Data.Migrations
{
    public partial class PropriedadeAtivoCursoEAluno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Curso",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Aluno",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Curso");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Aluno");
        }
    }
}
