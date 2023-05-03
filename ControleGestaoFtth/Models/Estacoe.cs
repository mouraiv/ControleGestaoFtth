using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleGestaoFtth.Models
{
    public class Estacoe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sigla { get; set; } = null!;
        [Required]
        public string NomeEstacao { get; set; } = null!;
        public int? EstadosId { get; set; }
        public Estado Estado { get; set; } = null!;
        public virtual ICollection<TesteOptico> TesteOptico { get; set; } = null!;

    }
}
