using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha.Models
{
    [Table("Participantes")]
    public class Participante
    {
        [Key]
        public short Matricula { get; set; }
        public DateTime Nascimento { get; set; }
        public string Sexo { get; set; }
        public short Id_Plano { get; set; }
        public short Id_Regra { get; set; }
        public DateTime? DIB { get; set; }
    }
}