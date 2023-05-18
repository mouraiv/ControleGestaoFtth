using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Tecnico
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Funcao { get; set; }
        public string? Email { get; set;}
        public Usuario usuario { get; set; } = null!;
    }
}
