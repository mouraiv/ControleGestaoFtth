using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Municipio
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? EstadoId { get; set; }
        public Estado Estado { get; set; } = null!;
    }
}
