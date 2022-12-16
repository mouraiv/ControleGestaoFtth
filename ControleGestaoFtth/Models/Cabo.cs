using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Cabo
    {
        [Key]
        public int Id { get; set; }
        public int? Quantidade { get; set; }
        public int? QuantidadeCelula { get; set; }
        public int? CapacidadeCelula { get; set; }

    }
}
