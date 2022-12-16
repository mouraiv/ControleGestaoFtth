using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Construtora
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DataAceitacao { get; set; }
        public string? MesRefAceitacao { get; set; }
        public string? Observacao { get; set; }
        public int? Validacao { get; set; }
        public string? Meta { get; set; }
        public DateTime? DataConstrucao { get; set; }
        public DateTime? DataTeste { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public int? BobinaLancamento { get; set; }
        public int? BobinaRecepcao { get; set; }
        public int? Quantidade { get; set; }
        public string? PosicaoIcxDgo  { get; set; }
        public int? SplitterCeos { get; set; }
        public int? FibraDgo { get; set; }
        public int? StatusId { get; set; }
        public State? States { get; set; }
        public int? CdoId { get; set; }
        public Cdo? Cdos { get; set; }

    }
}
