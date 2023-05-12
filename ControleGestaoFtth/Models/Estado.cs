using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Estado
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Uf { get; set; } = null!;
        public int RegiaoId { get; set; }
        public Regioe Regiao { get; set; } = null!;
        public virtual ICollection<Estacoe> Estacao { get; set; } = null!;

    }
}
