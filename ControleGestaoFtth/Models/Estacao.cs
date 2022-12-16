using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Estacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        [Required]
        public string? Sigla { get; set; }
        [Required]
        public string? TipoObra { get; set; }
        public int? CdoId { get; set; }
        public Cdo? Cdos { get; set; }
    }
}
