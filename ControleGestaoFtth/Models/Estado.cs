using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleGestaoFtth.Models
{
    public class Estado
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int RegiaoId { get; set; }
        public Regioe Regiao { get; set; } = null!;

    }
}
