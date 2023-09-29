namespace Escola.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int QtdMaxAlunos { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
