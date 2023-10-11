using Escola.Data;
using Escola.Models;
using Escola.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Escola.Services
{
    public class SvcMatricula : ISvcMatricula
    {
        private readonly ApplicationDbContext _context;

        public SvcMatricula(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Matricula?>> BuscarMatriculasAsync()
        {
            var matriculas =
                _context.Matricula
                .Include(x => x.Aluno)
                .Include(x => x.Curso)
                .ToListAsync();

            return await matriculas;
        }

        public async Task<List<Matricula?>> BuscarMatriculasFiltradasAsync(bool? ativa = true)
        {
            var matriculasAtivas =
                _context.Matricula
                .Include(x => x.Aluno)
                .Include(x => x.Curso)
                .Where(x => x.Ativa == ativa)
                .ToListAsync();

            return await matriculasAtivas;
        }

        public async Task<Matricula> BuscarMatriculaPorIdAsync(int id)
        {
            var matricula =
                await _context.Matricula
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(x => x.Id == id);

            return matricula;
        }

        public async Task<List<Matricula?>> BuscarMatriculaPorIdAluno(int id)
        {
            var matriculas = 
                await _context.Matricula
                .Include(x => x.Curso)
                .Include(x => x.Aluno)
                .Where(x => x.AlunoId == id).ToListAsync();

            return matriculas;
        }

        public async Task NovaMatriculaAsync(MatriculaViewModel matricula)
        {
            var novaMatricula = new Matricula
            {
                Id = matricula.Id,
                Ativa = true,
                AlunoId = matricula.AlunoId,
                CursoId = matricula.CursoId,
                DataCriacao = DateTime.Now
            };

            _context.Add(novaMatricula);
            await _context.SaveChangesAsync();
        }

        public async Task EditarMatriculaAsync(MatriculaViewModel matricula)
        {
            var matriculaEditada = BuscarMatriculaPorIdAsync(matricula.Id).Result;

            if (matriculaEditada != null)
            {
                matriculaEditada.AlunoId = matricula.AlunoId;
                matriculaEditada.CursoId = matricula.CursoId;
                matriculaEditada.DataAlteracao = DateTime.Now;

                _context.Update(matriculaEditada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task InativarMatriculaAsync(int id)
        {
            if (id > 0)
            {
                var matricula = BuscarMatriculaPorIdAsync(id).Result;
                if (matricula != null)
                {
                    matricula.Ativa = false;
                    _context.Matricula.Update(matricula);
                    await _context.SaveChangesAsync();
                }
            }
        }
        
        public async Task ReativarMatriculaAsync(int id)
        {
            if (id > 0)
            {
                var matricula = BuscarMatriculaPorIdAsync(id).Result;
                if (matricula != null)
                {
                    matricula.Ativa = true;
                    _context.Matricula.Update(matricula);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> ValidaMatriculaAtivaCursoAsync(int id)
        {
            var matriculas = await BuscarMatriculasFiltradasAsync(true);
            var matriculasAtivasCurso = matriculas.Where(x => x.CursoId == id);

            return matriculasAtivasCurso.Any();
        }
    }
}
