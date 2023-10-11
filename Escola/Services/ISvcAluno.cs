using Escola.Models;

namespace Escola.Services
{
    public interface ISvcAluno
    {
        Task<List<Aluno?>> BuscarAlunosAsync();
        Task<List<Aluno?>> BuscarAlunosFiltradosAsync(bool? ativo);
        Task<Aluno> BuscarAlunoPorIdAsync(int id);
        Task<Aluno> BuscarAlunoPorCpfAsync(string cpf);
        Task NovoAlunoAsync(Aluno aluno);
        Task EditarAlunoAsync(Aluno aluno);
        Task InativarAlunoAsync(int id);
        Task ReativarAlunoAsync(int id);
    }
}