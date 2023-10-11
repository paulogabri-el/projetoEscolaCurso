using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Escola.Data.Migrations
{
    public partial class AjusteOnDeleteFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula");

            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Curso_CursoId",
                table: "Matricula");

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula",
                column: "AlunoId",
                principalTable: "Aluno",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Curso_CursoId",
                table: "Matricula",
                column: "CursoId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula");

            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Curso_CursoId",
                table: "Matricula");

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula",
                column: "AlunoId",
                principalTable: "Aluno",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Curso_CursoId",
                table: "Matricula",
                column: "CursoId",
                principalTable: "Curso",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
