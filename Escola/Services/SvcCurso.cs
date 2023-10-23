using Escola.Data;
using Escola.Models;
using Microsoft.EntityFrameworkCore;

namespace Escola.Services
{
    public class SvcCurso : ISvcCurso
    {
        private readonly ApplicationDbContext _context;

        public SvcCurso(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task<List<Curso?>> BuscarCursosAsync()
        {
            var cursos = await _context.Curso.ToListAsync();

            return cursos;
        }

        public async Task<Curso> BuscarCursoPorIdAsync(int id)
        {
            var curso = await _context.Curso.FirstOrDefaultAsync(x => x.Id == id);

            return curso;
        }

        public async Task<List<Curso?>> BuscarCursosFiltradosAsync(bool? ativo = true)
        {
            var cursosFiltrados = await _context.Curso.Where(x => x.Ativo == ativo).ToListAsync();

            return cursosFiltrados;
        }        

        public async Task EditarCursoAsync(Curso curso)
        {
            if(curso != null)
            {
                var cursoEditar = BuscarCursoPorIdAsync(curso.Id).Result;

                cursoEditar.Id = curso.Id;
                cursoEditar.Descricao = curso.Descricao;
                cursoEditar.QtdMaxAlunos = curso.QtdMaxAlunos;
                cursoEditar.DataAlteracao = DateTime.Now;

                _context.Curso.Update(cursoEditar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task InativarCursoAsync(int id)
        {
            if(id > 0)
            {
                var cursoInativar = BuscarCursoPorIdAsync(id).Result;

                cursoInativar.Ativo = false;
                cursoInativar.DataAlteracao = DateTime.Now;

                _context.Curso.Update(cursoInativar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReativarCursoAsync(int id)
        {
            if (id > 0)
            {
                var cursoReativar = BuscarCursoPorIdAsync(id).Result;

                cursoReativar.Ativo = true;
                cursoReativar.DataAlteracao = DateTime.Now;

                _context.Curso.Update(cursoReativar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task NovoCursoAsync(Curso curso)
        {
            curso.DataCriacao = DateTime.Now;
            curso.Ativo = true;

            _context.Curso.Add(curso);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidaVagasDisponiveis(int id)
        {
            var curso = await BuscarCursoPorIdAsync(id);
            
            if(curso != null)
            {
                var vagas = curso.QtdMaxAlunos;
                var alunosAtivos = _context.Matricula.Where(x => x.CursoId == id && x.Ativa == true).Count();

                return vagas > alunosAtivos;
            }

            return false;
        }

        public async Task<bool> ValidaVagasDisponiveis(int id, int novaQuantidade)
        {
            var curso = await BuscarCursoPorIdAsync(id);

            if (curso != null)
            {
                var alunosAtivos = _context.Matricula.Where(x => x.CursoId == id && x.Ativa == true).Count();

                return novaQuantidade >= alunosAtivos;
            }

            return false;
        }
    }
}
