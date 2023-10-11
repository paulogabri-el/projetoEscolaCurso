using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Escola.Models
{
    public class Curso
    {
        public int Id { get; set; }
        [DisplayName("Ativo")]
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        [DisplayName("Quantidade de vagas")]
        public int QtdMaxAlunos { get; set; }
        [DisplayName("Data de criação")]
        [DataType(DataType.Date)]
        public DateTime? DataCriacao { get; set; }
        [DisplayName("Data de alteração")]
        [DataType(DataType.Date)]
        public DateTime? DataAlteracao { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}
