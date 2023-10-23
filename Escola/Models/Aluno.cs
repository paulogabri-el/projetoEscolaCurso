using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escola.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DisplayName("Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        [DisplayName("Data de criação")]
        [DataType(DataType.Date)]
        public DateTime? DataCriacao { get; set; }
        [DisplayName("Data de alteração")]
        [DataType(DataType.Date)]
        public DateTime? DataAlteracao { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
        public virtual string CpfFormatado
        {
            get
            {
                return $"{CPF[..3]}.{CPF.Substring(3, 3)}.{CPF.Substring(6, 3)}-{CPF[9..]}";
            }
        }
        
        public void FormataCpf (string cpf)
        {
            CPF = cpf.Replace(".", "").Replace("-", "");
        }
    }
}
