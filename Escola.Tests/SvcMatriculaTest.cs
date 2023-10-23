using Escola.Data;
using Escola.Models;
using Escola.Models.ViewModels;
using Escola.Services;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Escola.Tests
{
    public class SvcMatriculaTest
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        private readonly ApplicationDbContext context;
        private readonly Curso curso;
        private readonly List<Curso> cursos;
        private readonly Aluno aluno;
        private readonly List<Aluno> alunos;
        private readonly Matricula matricula;
        private readonly List<Matricula> matriculas;

        public SvcMatriculaTest()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new ApplicationDbContext(options);

            cursos = new List<Curso>
            {
                new Curso { Id = 10, Ativo = true, Descricao = "Curso1", QtdMaxAlunos = 10, DataCriacao = DateTime.Now},
                new Curso { Id = 20, Ativo = false, Descricao = "Curso2", QtdMaxAlunos = 20, DataCriacao = DateTime.Now}
            };

            curso = new Curso
            {
                Id = 30,
                Ativo = true,
                Descricao = "Curso3",
                QtdMaxAlunos = 30,
                DataCriacao = DateTime.Now
            };

            alunos = new List<Aluno>
            {
                new Aluno { Id = 10, Nome = "Aluno1", CPF = "39514291000", DataNascimento = new DateTime(2000, 01, 01), DataCriacao = DateTime.Now, Ativo = true },
                new Aluno { Id = 20, Nome = "Aluno2", CPF = "47785967067", DataNascimento = new DateTime(1999, 02, 02), DataCriacao = DateTime.Now, Ativo = false }
            };

            aluno = new Aluno
            {
                Id = 30,
                Nome = "Aluno1",
                CPF = "39514291000",
                DataNascimento = new DateTime(2000, 01, 01),
                DataCriacao = DateTime.Now
            };

            matriculas = new List<Matricula>
            {
                new Matricula { Id = 30, Ativa = true, AlunoId = 10 , CursoId = 10, DataCriacao = DateTime.Now},
                new Matricula { Id = 40, Ativa = false, AlunoId = 20 , CursoId = 20, DataCriacao = DateTime.Now}
            };

            matricula = new Matricula
            {
                Id = 30,
                Ativa = true,
                AlunoId = 30,
                CursoId = 30,
                DataCriacao = DateTime.Now
            };
        }

        [Fact]
        public async Task BuscarMatriculasAsync()
        {
            context.Curso.AddRange(cursos);
            context.Aluno.AddRange(alunos);
            context.Matricula.AddRange(matriculas);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);
            var result = await minhaClasse.BuscarMatriculasAsync();

            Assert.NotNull(result);
            Assert.Equal(10, result.FirstOrDefault(x => x.Id == 30).AlunoId);
            Assert.Equal(20, result.FirstOrDefault(x => x.Id == 40).AlunoId);

            context.Matricula.RemoveRange(matriculas);
            context.Aluno.RemoveRange(alunos);
            context.Curso.RemoveRange(cursos);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarMatriculasFiltradasAsync()
        {
            context.Curso.AddRange(cursos);
            context.Aluno.AddRange(alunos);
            context.Matricula.AddRange(matriculas);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);
            var result = await minhaClasse.BuscarMatriculasFiltradasAsync(true);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.True(result[0].Ativa);

            context.Matricula.RemoveRange(matriculas);
            context.Aluno.RemoveRange(alunos);
            context.Curso.RemoveRange(cursos);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarMatriculaPorIdAsync()
        {
            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            context.Matricula.Add(matricula);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);
            var result = await minhaClasse.BuscarMatriculaPorIdAsync(matricula.Id);

            Assert.NotNull(result);
            Assert.True(result.Ativa);
            Assert.Equal(30, result.CursoId);

            context.Matricula.Remove(matricula);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarMatriculaPorIdAluno()
        {
            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            context.Matricula.Add(matricula);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);
            var result = await minhaClasse.BuscarMatriculaPorIdAluno(aluno.Id);

            Assert.NotNull(result);
            Assert.True(result[0].Ativa);
            Assert.Equal(30, result[0].CursoId);

            context.Matricula.Remove(matricula);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task NovaMatriculaAsync()
        {
            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);
            var matriculaViewModel = new MatriculaViewModel
            {
                Id = matricula.Id,
                Ativa = matricula.Ativa,
                CursoId = matricula.CursoId,
                AlunoId = matricula.AlunoId
            };

            await minhaClasse.NovaMatriculaAsync(matriculaViewModel);
            var result = minhaClasse.BuscarMatriculaPorIdAsync(matricula.Id).Result;

            Assert.NotNull(result);
            Assert.True(result.Ativa);
            Assert.Equal(30, result.CursoId);

            context.Matricula.Remove(result);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task EditarMatriculaAsync()
        {
            context.Curso.AddRange(cursos);
            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            context.Matricula.Add(matricula);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);
            var matriculaViewModelEditar = new MatriculaViewModel
            {
                Id = matricula.Id,
                Ativa = matricula.Ativa,
                CursoId = 10,
                AlunoId = matricula.AlunoId
            };

            await minhaClasse.EditarMatriculaAsync(matriculaViewModelEditar);
            var result = minhaClasse.BuscarMatriculaPorIdAsync(matricula.Id).Result;

            Assert.NotNull(result);
            Assert.True(result.Ativa);
            Assert.Equal(10, result.CursoId);

            context.Matricula.Remove(matricula);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            context.Curso.RemoveRange(cursos);
            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task InativarMatriculaAsync()
        {
            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            context.Matricula.Add(matricula);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);

            await minhaClasse.InativarMatriculaAsync(matricula.Id);
            var result = minhaClasse.BuscarMatriculaPorIdAsync(matricula.Id).Result;

            Assert.NotNull(result);
            Assert.False(result.Ativa);

            context.Matricula.Remove(matricula);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task ReativarMatriculaAsync()
        {
            matricula.Ativa = false;

            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            context.Matricula.Add(matricula);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);

            await minhaClasse.ReativarMatriculaAsync(matricula.Id);
            var result = minhaClasse.BuscarMatriculaPorIdAsync(matricula.Id).Result;

            Assert.NotNull(result);
            Assert.True(result.Ativa);

            context.Matricula.Remove(matricula);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }
        
        [Fact]
        public async Task ValidaMatriculaAtivaCursoAsync()
        {
            context.Curso.Add(curso);
            context.Aluno.Add(aluno);
            context.Matricula.Add(matricula);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcMatricula(context);

            var result = minhaClasse.ValidaMatriculaAtivaCursoAsync(curso.Id).Result;

            Assert.True(result);

            context.Matricula.Remove(matricula);
            context.Aluno.Remove(aluno);
            context.Curso.Remove(curso);
            await context.SaveChangesAsync();
        }
    }
}