using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Escola.Data;
using Escola.Models;
using Escola.Services;
using Microsoft.AspNetCore.Authorization;

namespace Escola.Controllers
{
    [Authorize]
    public class CursoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISvcCurso _svcCurso;
        private readonly ISvcMatricula _svcMatricula;

        public CursoController(ApplicationDbContext context, ISvcCurso svc, ISvcMatricula svcMatricula)
        {
            _context = context;
            _svcCurso = svc;
            _svcMatricula = svcMatricula;
        }

        // GET: CursoController
        public async Task<IActionResult> Index()
        {
            try
            {
                var cursos = await _svcCurso.BuscarCursosFiltradosAsync(true);

                if (cursos.Count > 0)
                {
                    return View(cursos);
                }

                TempData["Informacao"] = "Não foi localizado nenhum curso ativo!";
                return View(cursos);
            }
            catch (Exception ex)
            {
                TempData["Informacao"] = $"Não foi possível localizar cursos, por favor, tente novamente! Detalhe do erro: {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> CursosInativos()
        {
            try
            {
                var cursos = await _svcCurso.BuscarCursosFiltradosAsync(false);

                if (cursos.Count() > 0)
                {
                    return View("CursosInativos", cursos);
                }

                TempData["Informacao"] = "Não foi localizado nenhum curso inativo!";
                return View(cursos);
            }
            catch (Exception ex)
            {
                TempData["Informacao"] = $"Não foi possível localizar cursos, por favor, tente novamente! Detalhe do erro: {ex.Message}";
                return View();
            }
        }

        // GET: CursoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var curso = await _svcCurso.BuscarCursoPorIdAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return View("_DetalhesCurso", curso);
        }

        // GET: CursoController/Create
        public IActionResult Create()
        {
            return View("_NovoCurso");
        }

        // POST: CursoController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Curso curso)
        {
            if (ModelState.IsValid)
            {
                await _svcCurso.NovoCursoAsync(curso);

                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // GET: CursoController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Curso == null)
            {
                return NotFound();
            }

            var curso = await _context.Curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            return View("_EditarCurso", curso);
        }

        // POST: CursoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var validaNovaQuantidade = await _svcCurso.ValidaVagasDisponiveis(id, curso.QtdMaxAlunos);
                    
                    if (!validaNovaQuantidade)
                    {
                        TempData["Informacao"] = "Existem mais matrículas ativas do que a nova quantidade de vagas informadas! Só é permitido alterar para uma quantiade igual ou superior à quantidade de matriculas ativas!";
                        return RedirectToAction(nameof(Index));
                    }
                    
                    await _svcCurso.EditarCursoAsync(curso);
                    TempData["Sucesso"] = $"Curso '{curso.Descricao}' editado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Erro"] = $"Não foi possível editar o curso, por favor, tente novamente! Detalhe do erro: {ex.Message}";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(curso);
        }

        // GET: CursoController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Curso == null)
            {
                return NotFound();
            }

            var curso = await _context.Curso
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View("_InativarCurso", curso);
        }

        // POST: CursoController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cursoMatriculaAtiva = await _svcMatricula.ValidaMatriculaAtivaCursoAsync(id);

                if (!cursoMatriculaAtiva)
                {
                    await _svcCurso.InativarCursoAsync(id);
                    TempData["Sucesso"] = "Curso inativado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Informacao"] = "Não foi possível inativar o curso pois existem matrículas ativas para ele!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível inativar o curso, tente novamente! Detalhe do erro: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Reativar(int id)
        {

            var curso = await _svcCurso.BuscarCursoPorIdAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return View("_ReativarCurso", curso);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarReativacao(int id)
        {
            try
            {
                var curso = await _svcCurso.BuscarCursoPorIdAsync(id);

                if (curso != null)
                {
                    await _svcCurso.ReativarCursoAsync(id);
                    TempData["Sucesso"] = "Curso reativado com sucesso!";
                    return RedirectToAction(nameof(CursosInativos));
                }

                TempData["Informacao"] = $"Não foi possível localizar o curso!";
                return RedirectToAction(nameof(CursosInativos));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível inativar o curso, tente novamente! Detalhe do erro: {ex.Message}";
                return RedirectToAction(nameof(CursosInativos));
            }
        }

        private bool CursoExists(int id)
        {
            return (_context.Curso?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
