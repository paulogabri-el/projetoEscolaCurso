using Microsoft.AspNetCore.Mvc;
using Escola.Data;
using Escola.Models;
using Escola.Services;
using DocumentValidator;
using Microsoft.AspNetCore.Authorization;

namespace Escola.Controllers
{
    [Authorize]
    public class AlunoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISvcAluno _svcAluno;
        private readonly ISvcMatricula _svcMatricula;

        public AlunoController(ApplicationDbContext context, ISvcAluno svcAluno, ISvcMatricula svcMatricula)
        {
            _context = context;
            _svcAluno = svcAluno;
            _svcMatricula = svcMatricula;
        }

        // GET: AlunoController
        public async Task<IActionResult> Index()
        {
            try
            {
                var alunos = await _svcAluno.BuscarAlunosFiltradosAsync(true);

                if (alunos.Count() > 0)
                {
                    return View(alunos);
                }

                TempData["Informacao"] = "Não foi localizado nenhum aluno ativo!";
                return View(alunos);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível localizar alunos, por favor, tente novamente! Detalhes do erro: {ex.Message}";
                return View();
            }
        }

        public async Task<IActionResult> AlunosInativos()
        {
            try
            {
                var alunos = await _svcAluno.BuscarAlunosFiltradosAsync(false);

                if (alunos.Count > 0)
                {
                    return View("AlunosInativos", alunos);
                }

                TempData["Informacao"] = "Não foi localizada nenhum aluno inativo!";
                return View("AlunosInativos", alunos);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível localizar alunos, por favor, tente novamente! Detalhes do erro: {ex.Message}";
                return View("AlunosInativos");
            }
        }

        // GET: AlunoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var aluno = await _svcAluno.BuscarAlunoPorIdAsync(id);

                return View("_DetalhesAluno", aluno);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi buscar detalhes do aluno, tente novamente! Detalhe do erro: {ex.Message}";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AlunoController/Create
        public IActionResult Create()
        {
            return View("_NovoAluno");
        }

        // POST: AlunoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Aluno aluno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var verificaAluno = _svcAluno.BuscarAlunoPorCpfAsync(aluno.CPF).Result;
                    if (verificaAluno != null)
                    {
                        TempData["Informacao"] = $"Já existe um aluno com o CPF informado! Nome: {verificaAluno.Nome}, cadastrado em: {verificaAluno.DataCriacao?.ToString("dd/MM/yyyy")}";
                        return RedirectToAction(nameof(Index));
                    }

                    if (!CpfValidation.Validate(aluno.CPF))
                    {
                        TempData["CpfInvalido"] = $"O CPF informado não é válido!";
                        return RedirectToAction(nameof(Index));
                    }

                    await _svcAluno.NovoAlunoAsync(aluno);
                    TempData["Sucesso"] = $"Aluno '{aluno.Nome}' cadastrado com sucesso!";

                    return RedirectToAction(nameof(Index));
                }

                return View("_NovoAluno", aluno);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível cadastrar o aluno, por favor, tente novamente! Detalhe do erro: {ex.Message}";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AlunoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id > 0)
            {
                var aluno = await _svcAluno.BuscarAlunoPorIdAsync(id);

                if (aluno == null)
                {
                    return NotFound();
                }

                return View("_editaraluno", aluno);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!CpfValidation.Validate(aluno.CPF))
                    {
                        TempData["CpfInvalido"] = $"O CPF informado não é válido!";
                        return RedirectToAction(nameof(Index));
                    }

                    await _svcAluno.EditarAlunoAsync(aluno);
                    TempData["Sucesso"] = $"Aluno '{aluno.Nome}' editado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Erro"] = $"Não foi possível editar o aluno, por favor, tente novamente! Detalhe do erro: {ex.Message}";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(aluno);
        }


        public async Task<IActionResult> Inativar(int id)
        {
            var aluno = await _svcAluno.BuscarAlunoPorIdAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return View("_InativarAluno", aluno);
        }

        // POST: AlunoController/Inativar/5
        [HttpPost, ActionName("Inativar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmaInativacao(int id)
        {
            try
            {
                var existeMatricula = await _svcMatricula.BuscarMatriculasFiltradasAsync(true);

                if (existeMatricula.FirstOrDefault(x => x.AlunoId == id) != null)
                {
                    TempData["Informacao"] = "Não é possível inativar o aluno pois existem matrículas ativas para ele!";
                    return RedirectToAction(nameof(Index));
                }

                await _svcAluno.InativarAlunoAsync(id);
                TempData["Sucesso"] = "Aluno inativado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível inativar o aluno, tente novamente! Detalhe do erro: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Reativar(int id)
        {
            var aluno = await _svcAluno.BuscarAlunoPorIdAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            return View("_ReativarAluno", aluno);
        }

        // POST: AlunoController/Reativar/5
        [HttpPost, ActionName("Reativar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmaReativacao(int id)
        {
            try
            {
                var aluno = _svcAluno.BuscarAlunoPorIdAsync(id).Result;
                await _svcAluno.ReativarAlunoAsync(id);
                TempData["Sucesso"] = $"Aluno '{aluno.Nome}' reativado com sucesso!";
                return RedirectToAction(nameof(AlunosInativos));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Não foi possível reativar o aluno, tente novamente! Detalhe do erro: {ex.Message}";
                return RedirectToAction(nameof(AlunosInativos));
            }
        }

        [AllowAnonymous]
        public async Task <IActionResult> SouAluno()
        {
            return View("_SouAluno");
        }

        [AllowAnonymous]
        public async Task<IActionResult> SouAlunoList(string cpf, DateTime dataNascimento)
        {
            try
            {
                var aluno = await _svcAluno.BuscarAlunoPorCpfAsync(cpf);
                if (aluno != null && aluno.DataNascimento.Date == dataNascimento.Date)
                {
                    var matriculas = _svcMatricula.BuscarMatriculaPorIdAluno(aluno.Id).Result;
                    if (matriculas != null)

                    {
                        return View("SouAlunoMatriculas", matriculas);
                    }

                    TempData["Informacao"] = $"Não existem matrículas ativas para esse aluno!";
                    return RedirectToAction(nameof(SouAluno));
                }

                TempData["Informacao"] = $"Aluno não encontrado. Verifique o CPF e a data de nascimento!";
                return RedirectToAction(nameof(SouAluno));

            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool AlunoExists(int id)
        {
            return (_context.Aluno?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
