using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha.Models
{
    [Table("TipoBeneficio")]
    public class TipoBeneficio
    {
        [Key]
        public byte IdBeneficio { get; set; }
        public string Nome { get; set; }
    }

}