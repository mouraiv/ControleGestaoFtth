using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class States
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? EstadodeProjeto { get; set; }
        [Required]
        public string? EstadodeControle { get; set; }
        [Required]
        public string? Descricao { get; set; }
    }
}
