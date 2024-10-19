using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Alpha.Models
{
    [Table("Plano")]

    public class Plano
    {
        [Key]
        public byte IdPlano { get; set; }
        public string Nome { get; set; }
    }
}