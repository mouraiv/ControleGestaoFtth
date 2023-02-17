using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Cdo
    {
        [Key]
        public int Id { get; set; }
        public Estacoe Estacao { get; set; } = null!;
        public int? EstacoesId { get; set; }
        public TipoObra TipoObra { get; set; } = null!;
        public int? TipoObraId { get; set; }
        public string CDO { get; set; } = null!;
        public int Cabo { get; set; }
        public int? Celula { get; set; }
        public int? Capacidade { get; set; }
        public int? TotalUms { get; set; }
        public string? Endereco { get; set; }
    }
}
