using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Escola.Data;
using Escola.Models;
using Escola.Models.ViewModels;
using System.Runtime.CompilerServices;
using Escola.Services;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace Escola.Controllers
{
    [Authorize]
    public class MatriculaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISvcMatricula _svcMatricula;
        private readonly ISvcCurso _svcCurso;
        private readonly ISvcAluno _svcAluno;

        public MatriculaController(ApplicationDbContext context, ISvcMatricula svcMatricula, ISvcCurso svcCurso, ISvcAluno svcAluno)
        {
            _context = context;
            _svcMatricula = svcMatricula;
            _svcCurso = svcCurso;
            _svcAluno = svcAluno;
        }

        // GET: MatriculaController
        public async Task<IActionResult> Index()
        {
            try
            {
                var matriculas = await _svcMatricula.BuscarMatriculasFiltradasAsync(true);

                if (matriculas.Count() > 0)
                {
                    return View(matriculas);
                }

                TempData["Informacao"] = "Não foi localizada nenhuma matrícula ativa!";
                return View(matriculas);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível localizar matrículas, por favor, tente novamente! Detalhes do erro: {ex.Message}";
                return View();
            }
        }
        
        public async Task<IActionResult> ReativarMatricula()
        {
            try
            {
                var matriculas = await _svcMatricula.BuscarMatriculasFiltradasAsync(false);

                if (matriculas.Count() > 0)
                {
                    return View("MatriculasInativas", matriculas);
                }

                TempData["Informacao"] = "Não foi localizada nenhuma matrícula inativa!";
                return View("MatriculasInativas", matriculas);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível localizar matrículas, por favor, tente novamente! Detalhes do erro: {ex.Message}";
                return View("MatriculasInativas");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var matricula = await _svcMatricula.BuscarMatriculaPorIdAsync(id);

                return View("_DetalhesMatricula", matricula);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi buscar detalhes da matrícula, tente novamente! Detalhe do erro: {ex.Message}";

                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            var resultAlunos = _svcAluno.BuscarAlunosFiltradosAsync(true).Result;
            var resultCursos = _svcCurso.BuscarCursosFiltradosAsync(true).Result;
            ViewData["AlunoId"] = new SelectList(resultAlunos, "Id", "Nome");
            ViewData["CursoId"] = new SelectList(resultCursos, "Id", "Descricao");

            return View("_NovaMatricula");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatriculaViewModel matriculaViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var matricula = _svcMatricula.BuscarMatriculaPorIdAluno(matriculaViewModel.AlunoId).Result;
                    var validaMatricula = matricula.FirstOrDefault(x => x.Ativa == true && x.CursoId == matriculaViewModel.CursoId);
                    if(validaMatricula != null)
                    {
                        TempData["Informacao"] = "Não é possível cadastrar um aluno no mesmo curso várias vezes, por favor, escolha outro curso!";
                        return RedirectToAction(nameof(Index));
                    }

                    var validaQuantidadeVagas = _svcCurso.ValidaVagasDisponiveis(matriculaViewModel.CursoId).Result;

                    if (validaQuantidadeVagas)
                    {
                        await _svcMatricula.NovaMatriculaAsync(matriculaViewModel);
                        TempData["Sucesso"] = "Matrícula realizada com sucesso!";

                        return RedirectToAction(nameof(Index));
                    }

                    TempData["Informacao"] = "O curso escolhido não tem vagas disponíveis!";

                    return RedirectToAction(nameof(Index));

                }

                ViewData["AlunoId"] = new SelectList(_context.Aluno, "Id", "Nome", matriculaViewModel.AlunoId);
                ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Descricao", matriculaViewModel.CursoId);

                return View("_NovaMatricula", matriculaViewModel);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível realizar a matrícula, tente novamente! Detalhe do erro: {ex.Message}";

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (id > 0)
            {
                var matricula = await _svcMatricula.BuscarMatriculaPorIdAsync(id);

                if (matricula == null)
                {
                    return NotFound();
                }

                ViewData["AlunoId"] = new SelectList(_context.Aluno, "Id", "Nome", matricula.AlunoId);
                ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Descricao", matricula.CursoId);

                return View("_EditarMatricula", matricula);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, MatriculaViewModel matricula)
        {
            if (id != matricula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var matriculaExistente = _svcMatricula.BuscarMatriculaPorIdAluno(matricula.AlunoId).Result;
                    var validaMatricula = matriculaExistente.FirstOrDefault(x => x.Ativa == true && x.CursoId == matricula.CursoId);
                    if(validaMatricula != null)
                    {
                        TempData["Informacao"] = "Já existe uma matrícula ativa deste aluno para este curso!";
                        return RedirectToAction(nameof(Index));
                    }

                    await _svcMatricula.EditarMatriculaAsync(matricula);
                    TempData["Sucesso"] = "Matrícula editada com sucesso!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!MatriculaExists(matricula.Id))
                    {
                        TempData["Erro"] = "Matrícula não encontrada!";
                    }
                    else
                    {
                        TempData["Erro"] = $"Não foi possível editar a matrícula, por favor, tente novamente! Detalhe do erro: {ex.Message}";
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["AlunoId"] = new SelectList(_context.Aluno, "Id", "Nome", matricula.AlunoId);
            ViewData["CursoId"] = new SelectList(_context.Curso, "Id", "Descricao", matricula.CursoId);
            return View("_EditarMatricula", matricula);
        }

        // GET: MatriculaController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var matricula = await _svcMatricula.BuscarMatriculaPorIdAsync(id);

            if (matricula == null)
            {
                return NotFound();
            }

            return View("_InativarMatricula", matricula);
        }

        // POST: MatriculaController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _svcMatricula.InativarMatriculaAsync(id);
                TempData["Sucesso"] = "Matrícula inativada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível inativar a matrícula, tente novamente! Detalhe do erro: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Reativar(int id)
        {

            var matricula = await _svcMatricula.BuscarMatriculaPorIdAsync(id);

            if (matricula == null)
            {
                return NotFound();
            }

            return View("_ReativarMatricula", matricula);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarReativacao(int id)
        {
            try
            {
                var matricula = await _svcMatricula.BuscarMatriculaPorIdAsync(id);

                if (matricula != null)
                {
                    var validaVagasDisponiveis = await _svcCurso.ValidaVagasDisponiveis(matricula.CursoId);

                    if (validaVagasDisponiveis)
                    {
                        await _svcMatricula.ReativarMatriculaAsync(id);
                        TempData["Sucesso"] = "Matrícula reativada com sucesso!";
                        return RedirectToAction(nameof(ReativarMatricula));
                    }

                    var curso = _svcCurso.BuscarCursoPorIdAsync(matricula.CursoId).Result;
                    TempData["Informacao"] = $"Não foi possível reativar a matrícula porquê o curso '{curso.Descricao}' não tem vagas disponíveis!";
                    return RedirectToAction(nameof(ReativarMatricula));
                }

                TempData["Informacao"] = $"Não foi possível localizar a matrícula!";
                return RedirectToAction(nameof(ReativarMatricula));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível inativar a matrícula, tente novamente! Detalhe do erro: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool MatriculaExists(int id)
        {
            return (_context.Matricula?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
