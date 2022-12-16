using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? EstadoProjeto { get; set; }
        [Required]
        public string? EstadoControle { get; set; }
        [Required]
        public string? EstadoCampo { get; set; }
    }
}
