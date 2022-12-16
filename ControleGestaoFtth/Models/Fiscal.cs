using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Fiscal
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DataEnvio { get; set; }
        [Required]
        public DateTime DataEn { get; set; }
    }
}
