using System.ComponentModel.DataAnnotations.Schema;

namespace Escola.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
