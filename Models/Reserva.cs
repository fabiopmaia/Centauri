using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha.Models
{
    [Table("Reservas")]
    public class Reserva
    {
        public DateTime DataRef { get; set; }
        public byte Id_Vinculo { get; set; }
        public double? Salario { get; set; }
        public double? Percentual { get; set; }
        public double? ContribMain { get; set; }
        public double? ContribAdd { get; set; }
        public double? Principal { get; set; }
        public byte? Forma { get; set; }
        public short? Prazo { get; set; }
        public double? Renda { get; set; }
        public double? Adicional { get; set; }
        public byte? Forma1 { get; set; }
        public double? Renda1 { get; set; }
        public byte? Forma2 { get; set; }
        public double? Renda2 { get; set; }
        [Key]
        public short Id_Matricula { get; set; }
    }
}