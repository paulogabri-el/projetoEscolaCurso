using Escola.Models;
using Escola.Models.ViewModels;

namespace Escola.Services
{
    public interface ISvcMatricula
    {
        Task<List<Matricula?>> BuscarMatriculasAsync();
        Task<List<Matricula?>> BuscarMatriculasFiltradasAsync(bool? ativa);
        Task<Matricula> BuscarMatriculaPorIdAsync(int id);
        Task<List<Matricula?>> BuscarMatriculaPorIdAluno(int id);
        Task NovaMatriculaAsync(MatriculaViewModel matricula);
        Task EditarMatriculaAsync(MatriculaViewModel matricula);
        Task InativarMatriculaAsync(int id);
        Task ReativarMatriculaAsync(int id);
        Task<bool> ValidaMatriculaAtivaCursoAsync(int id);
    }
}
