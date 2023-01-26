using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class TipoObra
    {
        [Key]
        public int? Id { get; set; }
        public string? Nome { get; set; } = null!;
    }
}
