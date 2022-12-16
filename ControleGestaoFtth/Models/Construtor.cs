using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Construtor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        public int ? EstacaoId { get; set; }
        public Estacao? Estacaos { get; set; }
    }
}
