using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Analise
    {
        [Key]
        public int Id { get; set; }
        public int? TesteOpticoId { get; set; }
        public TesteOptico TesteOptico { get; set; } = null!;
        public int? Status { get; set; }
        public int? TecnicoId { get; set; }
        public Tecnico Tecnico { get; set; } = null!;
        public DateTime DataAnalise { get; set; }
        public string? Observacao { get; set; } 
        public string? CDOIA { get; set; }
        public string? CDOIAStatus { get; set; }
        public string? CDOIA_Obs { get; set; }

        public virtual ICollection<Cdoia> Cdoias { get; set; } = null!;
    }
}
