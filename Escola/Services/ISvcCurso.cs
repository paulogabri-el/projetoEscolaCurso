using Escola.Models;

namespace Escola.Services
{
    public interface ISvcCurso
    {
        Task<List<Curso?>> BuscarCursosAsync();
        Task<List<Curso?>> BuscarCursosFiltradosAsync(bool? ativo);
        Task<Curso> BuscarCursoPorIdAsync(int id);
        Task NovoCursoAsync(Curso curso);
        Task EditarCursoAsync(Curso curso);
        Task InativarCursoAsync(int id);
        Task<bool> ValidaVagasDisponiveis(int id);
        Task<bool> ValidaVagasDisponiveis(int id, int novaQuantidade);
        Task ReativarCursoAsync(int id);
    }
}