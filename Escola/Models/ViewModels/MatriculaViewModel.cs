namespace Escola.Models.ViewModels
{
    public class MatriculaViewModel
    {
            public int Id { get; set; }
            public bool Ativa { get; set; }
            public int AlunoId { get; set; }
            public int CursoId { get; set; }
            public string? Aluno {  get; set; }
            public string? Curso { get; set; }
    }
}
