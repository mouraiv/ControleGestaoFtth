using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Estacoe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Sigla { get; set; } = null!;
        [Required]
        public string NomeEstacao { get; set; } = null!;
        [Required]
        public string? Responsavel { get; set; } = null!;
    }
}
