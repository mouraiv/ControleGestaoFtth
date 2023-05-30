using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Cdoia
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? Status { get; set; } 
        public string? Observacao { get; set; }
        public int? AnaliseId { get; set; }
        public Analise Analise { get; set; } = null!;
    }
}
