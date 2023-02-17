using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class TesteOptico
    {
        [Key]
        public int Id { get; set; }
        public EstadoCampo? EstadoCampo { get; set; }
        public int? EstadoCamposId { get; set; }
        public State State { get; set; } = null!;
        public int? StatesId { get; set; }
        public DateTime? AceitacaoData { get; set; }
        public string? AceitacaoMesRef { get; set; }
        public string? Observacoes { get; set; }
        public int? NetwinId { get; set; }
        public Netwin Netwin { get; set; } = null!;
        public string? Meta { get; set; }
        public DateTime? DatadeConstrucao { get; set; }
        public string? EquipedeConstrucao { get; set; }
        public DateTime? DatadoTeste { get; set; }
        public string? Tecnico { get; set; }
        public DateTime? DatadeRecebimento { get; set; }
        public int? BobinadeLancamento { get; set; }
        public int? BobinadeRecepcao { get; set; }
        public int? QuantidadeDeTeste { get; set; }
        public string? PosicaoICX_DGO { get; set; }
        public string? SplitterCEOS { get; set; }
        public string? FibraDGO { get; set; }
        public Cdo Cdo { get; set; } = null!;
        public int? CdosId { get; set; }
    }
}
