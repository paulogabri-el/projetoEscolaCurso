using Escola.Data;
using Escola.Models;
using Escola.Services;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Escola.Tests
{
    public class SvcCursoTest
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        private readonly ApplicationDbContext context;
        private readonly Curso curso;
        private readonly List<Curso> cursos;

        public SvcCursoTest()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new ApplicationDbContext(options);

            cursos = new List<Curso>
                    {
                        new Curso { Id = 1, Ativo = true, Descricao = "Curso1", QtdMaxAlunos = 10, DataCriacao = DateTime.Now},
                        new Curso { Id = 2, Ativo = false, Descricao = "Curso2", QtdMaxAlunos = 20, DataCriacao = DateTime.Now}
                    };

            curso = new Curso
            {
                Id = 3,
                Ativo = true,
                Descricao = "Curso3",
                QtdMaxAlunos = 30,
                DataCriacao = DateTime.Now
            };
        }

        [Fact]
        public async Task BuscarCursosAsync()
        {
            context.Curso.AddRange(cursos);
            await context.SaveChangesAsync();

            List<Curso> result;
            var minhaClasse = new SvcCurso(context);
            result = await minhaClasse.BuscarCursosAsync();

            Assert.NotNull(result);
            Assert.Equal("Curso1", result.FirstOrDefault(x => x.Id == 1).Descricao);
            Assert.Equal("Curso2", result.FirstOrDefault(x => x.Id == 2).Descricao);

            context.Curso.RemoveRange(cursos);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarCursoPorIdAsync()
        {
            context.Curso.Add(curso);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcCurso(context);
            var result = await minhaClasse.BuscarCursoPorIdAsync(curso.Id);

            Assert.NotNull(result);
            Assert.Equal("Curso3", result.Descricao);

            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarCursosFiltradosAsync()
        {
            context.Curso.AddRange(cursos);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcCurso(context);
            var result = await minhaClasse.BuscarCursosFiltradosAsync(false);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.False(result.FirstOrDefault().Ativo);

            context.Curso.RemoveRange(cursos);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task EditarCursoAsync()
        {
            context.Curso.Add(curso);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcCurso(context);
            curso.Descricao = "NovoNomeTest";
            await minhaClasse.EditarCursoAsync(curso);
            var result = minhaClasse.BuscarCursoPorIdAsync(curso.Id).Result;

            Assert.NotNull(result);
            Assert.Equal("NovoNomeTest", result.Descricao);

            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task InativarCursoAsync()
        {
            context.Curso.Add(curso);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcCurso(context);
            await minhaClasse.InativarCursoAsync(curso.Id);
            var result = minhaClasse.BuscarCursoPorIdAsync(curso.Id).Result;

            Assert.NotNull(result);
            Assert.False(result.Ativo);

            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ReativarCursoAsync()
        {
            curso.Ativo = false;
            context.Curso.Add(curso);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcCurso(context);
            await minhaClasse.ReativarCursoAsync(curso.Id);
            var result = minhaClasse.BuscarCursoPorIdAsync(curso.Id).Result;

            Assert.NotNull(result);
            Assert.True(result.Ativo);

            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task NovoCursoAsync()
        {
            var minhaClasse = new SvcCurso(context);
            await minhaClasse.NovoCursoAsync(curso);
            var result = minhaClasse.BuscarCursoPorIdAsync(curso.Id).Result;

            Assert.NotNull(result);
            Assert.Equal("Curso3", result.Descricao);

            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ValidaVagasDisponiveis()
        {
            var aluno = new Aluno
            {
                Id = 50,
                Nome = "Aluno1",
                CPF = "39514291000",
                DataNascimento = new DateTime(2000, 01, 01),
                DataCriacao = DateTime.Now
            };

            var matricula = new Matricula
            {
                Id = 100,
                Ativa = true,
                AlunoId = aluno.Id,
                CursoId = curso.Id,
                DataCriacao = DateTime.Now
            };

            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            context.Matricula.Add(matricula);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcCurso(context);

            var result = minhaClasse.ValidaVagasDisponiveis(curso.Id).Result;
            Assert.True(result);

            result = minhaClasse.ValidaVagasDisponiveis(curso.Id, 0).Result;
            Assert.False(result);

            context.Matricula.Remove(matricula);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }
    }
}