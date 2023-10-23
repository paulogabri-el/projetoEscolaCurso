using Escola.Data;
using Escola.Models;
using Escola.Services;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Escola.Tests
{
    public class SvcAlunoTest
    {
        private readonly DbContextOptions<ApplicationDbContext> options;
        private readonly ApplicationDbContext context;
        private readonly Aluno aluno;
        private readonly List<Aluno> alunos;

        public SvcAlunoTest()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new ApplicationDbContext(options);

            alunos = new List<Aluno>
                    {
                        new Aluno { Id = 1, Nome = "Aluno1", CPF = "39514291000", DataNascimento = new DateTime(2000, 01, 01), DataCriacao = DateTime.Now, Ativo = true },
                        new Aluno { Id = 2, Nome = "Aluno2", CPF = "47785967067", DataNascimento = new DateTime(1999, 02, 02), DataCriacao = DateTime.Now, Ativo = false }
                    };

            aluno = new Aluno
            {
                Id = 3,
                Nome = "Aluno1",
                CPF = "39514291000",
                DataNascimento = new DateTime(2000, 01, 01),
                DataCriacao = DateTime.Now
            };
        }

        [Fact]
        public async Task BuscarAlunosAsync()
        {
            context.Aluno.AddRange(alunos);
            await context.SaveChangesAsync();

            List<Aluno> result;
            var minhaClasse = new SvcAluno(context);
            result = await minhaClasse.BuscarAlunosAsync();

            Assert.NotNull(result);
            Assert.Equal("Aluno1", result.FirstOrDefault(x => x.Id == 1).Nome);
            Assert.Equal("Aluno2", result.FirstOrDefault(x => x.Id == 2).Nome);

            context.Aluno.RemoveRange(alunos);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarAlunoPorIdAsync()
        {
            context.Aluno.Add(aluno);
            await context.SaveChangesAsync();

            Aluno result;
            var id = 3;
            var minhaClasse = new SvcAluno(context);
            result = await minhaClasse.BuscarAlunoPorIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal("Aluno1", result.Nome);

            context.Aluno.Remove(aluno);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarAlunoPorCpfAsync()
        {
            context.Aluno.Add(aluno);
            await context.SaveChangesAsync();

            Aluno result;
            var cpf = "39514291000";
            var minhaClasse = new SvcAluno(context);
            result = await minhaClasse.BuscarAlunoPorCpfAsync(cpf);

            Assert.NotNull(result);
            Assert.Equal("Aluno1", result.Nome);

            context.Aluno.Remove(aluno);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task BuscarAlunosFiltradosAsync()
        {
            context.Aluno.AddRange(alunos);
            await context.SaveChangesAsync();

            List<Aluno> result;
            var filtro = true;
            var minhaClasse = new SvcAluno(context);
            result = await minhaClasse.BuscarAlunosFiltradosAsync(filtro);

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal("Aluno1", result[0].Nome);

            context.Aluno.RemoveRange(alunos);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task EditarAlunoAsync()
        {
            context.Aluno.Add(aluno);
            await context.SaveChangesAsync();

            var editarAluno = new Aluno
            {
                Id = aluno.Id,
                Nome = "TesteEditar",
                CPF = aluno.CPF,
                DataNascimento = aluno.DataNascimento,
                DataCriacao = aluno.DataCriacao
            };

            var minhaClasse = new SvcAluno(context);
            await minhaClasse.EditarAlunoAsync(editarAluno);
            var result = await minhaClasse.BuscarAlunoPorIdAsync(aluno.Id);

            Assert.NotNull(aluno);
            Assert.Equal("TesteEditar", result.Nome);

            context.Aluno.Remove(aluno);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task InativarAlunoAsync()
        {
            context.Aluno.Add(aluno);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcAluno(context);
            await minhaClasse.InativarAlunoAsync(aluno.Id);
            var result = await minhaClasse.BuscarAlunoPorIdAsync(aluno.Id);

            Assert.False(result.Ativo);

            context.Aluno.Remove(aluno);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task NovoAlunoAsync()
        {
            var minhaClasse = new SvcAluno(context);
            await minhaClasse.NovoAlunoAsync(aluno);
            var result = await minhaClasse.BuscarAlunoPorIdAsync(aluno.Id);

            Assert.Equal(3, result.Id);

            context.Aluno.Remove(aluno);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ReativarAlunoAsync()
        {
            aluno.Ativo = false;
            context.Aluno.Add(aluno);
            await context.SaveChangesAsync();

            var minhaClasse = new SvcAluno(context);
            await minhaClasse.ReativarAlunoAsync(aluno.Id);
            var result = await minhaClasse.BuscarAlunoPorIdAsync(aluno.Id);

            Assert.True(result.Ativo);

            context.Aluno.Remove(aluno);
            await context.SaveChangesAsync();
        }

    }
}