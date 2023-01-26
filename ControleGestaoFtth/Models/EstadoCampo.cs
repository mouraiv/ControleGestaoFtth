using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class EstadoCampo
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; } = null!;
    }
}
