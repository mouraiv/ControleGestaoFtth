using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Equipe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        public int? ConstrutoraId { get; set; }
        public Construtora? Construtoras { get; set; }
    }
}
