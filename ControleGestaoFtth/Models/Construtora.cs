using System;
using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Construtora
    {
        [Key]
        public int Id { get; set; }
        public string? CHAVE { get; set; }
        public Estacoe? Estacao { get; set; }
        public int? EstacoesId { get; set; }
        public TipoObra? TipoObra { get; set; }
        public int? TipoObraId { get; set; }
        public string? CDO { get; set; }
        public int? Cabo { get; set; }
        public int? Celula { get; set; }
        public int? Capacidade { get; set; }
        public int? TotalUms { get; set; }
        public string? Endereco { get; set; }
        public EstadoCampo? EstadoCampo { get; set; }
        public int? EstadoCamposId { get; set; }
        public State? State { get; set; }
        public int? StatesId { get; set; }
        public DateTime? AceitacaoData { get; set; }
        public string? AceitacaoMesRef { get; set; }
        public string? Observacoes { get; set; }
        public Int16? Viabilidade { get; set; }
        public string? Meta { get; set; }
        public DateTime? DatadeConstrucao { get; set; }
        public string? EquipedeConstrucao { get; set; }
        public DateTime? DatadoTeste { get; set; }
        public string? Tecnico { get; set; }
        public DateTime? DatadeRecebimento { get; set; }
        public int? BobinadeLancamento { get; set; }
        public int? BobinadeRecepcao { get; set; }
        public int? QuantidadeDeTeste { get; set; }
        public string? PosicaoICX_DGO  { get; set; }
        public string? SplitterCEOS { get; set; }
        public string? FibraDGO { get; set; }
    }
}
