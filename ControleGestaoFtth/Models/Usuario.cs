using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get;set; }
        [Required]
        public string? Login { get;set; }
        [Required]
        public string? Senha { get;set; }
        [Required]
        public string? Tipo { get;set;}
        [Required]
        public int? Externo { get; set; }
    }
}
