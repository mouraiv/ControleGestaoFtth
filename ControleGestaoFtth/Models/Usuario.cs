using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get;set; }
        public string? Login { get;set; }
        public string? Senha { get;set; }
        public string? Tipo { get;set;}
        public int? Externo { get; set; }
        public Tecnico Tecnico { get; set; } = null!;
    }
}
