using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class EstadoCampos
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
    }
}
