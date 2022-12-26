using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Tecnicos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        [Required]
        public string? Funcao { get; set; }
        [Required]
        public string? Email { get; set;}
    }
}
