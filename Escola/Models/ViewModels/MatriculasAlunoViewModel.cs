using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Escola.Models.ViewModels
{
    public class MatriculasAlunoViewModel
    {
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [DisplayName("Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
    }
}
