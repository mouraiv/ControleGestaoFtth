using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class EstadoCampo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
    }
}
