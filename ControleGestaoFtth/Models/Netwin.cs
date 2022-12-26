using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Netwin
    {
        [Key]
        public int? Id { get; set; }
        public int? Codigo { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
    }
}
