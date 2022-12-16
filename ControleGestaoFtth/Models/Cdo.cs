using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Cdo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        public int? CaboId { get; set; }
        public Cabo? Cabos { get; set; }
        public int? UmsId { get; set; }
        public Ums? Umss { get; set; }
    }
}
