using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Escola.Models
{
    public class Matricula
    {
        public int Id { get; set; }

        [DisplayName("Ativa")]
        public bool Ativa { get; set; }

        [DisplayName("Data de criação")]
        [DataType(DataType.Date)]
        public DateTime? DataCriacao { get; set; }

        [DisplayName("Data de alteração")]
        [DataType(DataType.Date)]
        public DateTime? DataAlteracao { get; set; }
        public int AlunoId { get; set; }

        [DisplayName("Aluno")]
        public virtual Aluno Aluno { get; set; }
        public int CursoId { get; set; }

        [DisplayName("Curso")]
        public virtual Curso Curso { get; set; }
    }
}
