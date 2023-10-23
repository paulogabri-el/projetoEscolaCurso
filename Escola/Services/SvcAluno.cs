using Escola.Data;
using Escola.Models;
using Microsoft.EntityFrameworkCore;

namespace Escola.Services
{
    public class SvcAluno : ISvcAluno
    {
        private readonly ApplicationDbContext _context;

        public SvcAluno(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Aluno?>> BuscarAlunosAsync()
        {
            var alunos = await _context.Aluno.ToListAsync();

            return alunos;
        }

        public async Task<Aluno> BuscarAlunoPorIdAsync(int id)
        {
            var aluno = await _context.Aluno.FirstOrDefaultAsync(x => x.Id == id);

            return aluno;
        }

        public async Task<Aluno> BuscarAlunoPorCpfAsync(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");
            var aluno = await _context.Aluno.FirstOrDefaultAsync(x => x.CPF.Replace(".", "").Replace("-", "") == cpf);

            return aluno;
        }

        public async Task<List<Aluno?>> BuscarAlunosFiltradosAsync(bool? ativo)
        {
            var alunosFiltrados = await _context.Aluno.Where(x => x.Ativo == ativo).ToListAsync();

            return alunosFiltrados;
        }

        public async Task EditarAlunoAsync(Aluno aluno)
        {
            var alunoEditar = await BuscarAlunoPorIdAsync(aluno.Id);

            alunoEditar.Nome = aluno.Nome;
            aluno.FormataCpf(aluno.CPF);
            alunoEditar.CPF = aluno.CPF; 
            alunoEditar.DataNascimento = aluno.DataNascimento;
            alunoEditar.DataAlteracao = DateTime.Now;

            _context.Aluno.Update(alunoEditar);
            await _context.SaveChangesAsync();
        }

        public async Task InativarAlunoAsync(int id)
        {
            var alunoInativar = await BuscarAlunoPorIdAsync(id);

            alunoInativar.Ativo = false;
            alunoInativar.DataAlteracao = DateTime.Now;

            _context.Aluno.Update(alunoInativar);
            await _context.SaveChangesAsync();
        }

        public async Task NovoAlunoAsync(Aluno aluno)
        {
            aluno.DataCriacao = DateTime.Now;
            aluno.Ativo = true;
            aluno.FormataCpf(aluno.CPF);

            _context.Aluno.Add(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task ReativarAlunoAsync(int id)
        {
            var alunoReativar = await BuscarAlunoPorIdAsync(id);

            alunoReativar.Ativo = true;
            alunoReativar.DataAlteracao = DateTime.Now;

            _context.Aluno.Update(alunoReativar);
            await _context.SaveChangesAsync();
        }
    }
}
