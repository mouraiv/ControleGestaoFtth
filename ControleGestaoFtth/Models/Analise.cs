using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Analise
    {
        [Key]
        public int? Id { get; set; }
        public DateTime? DateAnalise { get; set;}
        public string? Observacao { get; set; }
        public string? Cdoia { get; set; }
        public string? CdoiaStatus { get; set; }
        public string? CdoiaObs { get; set; }
        public int? TecnicoId { get; set; }
        public Tecnico? Tecnicos { get; set; }
        public int? CdoId { get; set; }
        public Cdo? Cdos { get; set; }
    }
}
