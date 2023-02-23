using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Construtora
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
    }
}
