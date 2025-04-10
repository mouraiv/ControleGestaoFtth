﻿using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Enderecostotais
    {
        [Key]
        public int Id { get; set; }
        public string? CELULA { get; set; }
        public string? ESTACAO_ABASTECEDORA { get; set; }
        public string? UF { get; set; }
        public string? MUNICIPIO { get; set; }
        public string? LOCALIDADE { get; set; }
        public string? COD_LOCALIDADE { get; set; }
        public string? LOCALIDADE_ABREV { get; set; }
        public string? LOGRADOURO { get; set; }
        public string? COD_LOGRADOURO { get; set; }
        public string? NUM_FACHADA { get; set; }
        public string? COMPLEMENTO { get; set; }
        public string? COMPLEMENTO2 { get; set; }
        public string? COMPLEMENTO3 { get; set; }
        public string? CEP { get; set; }
        public string? BAIRRO { get; set; }
        public string? COD_SURVEY { get; set; }
        public int? QUANTIDADE_UMS { get; set; }
        public int? COD_VIABILIDADE { get; set; }
        public string? TIPO_VIABILIDADE { get; set; }
        public string? TIPO_REDE { get; set; }
        public string? UCS_RESIDENCIAIS { get; set; }
        public string? UCS_COMERCIAIS { get; set; }
        public string NOME_CDO { get; set; } = null!;
        public int ID_ENDERECO { get; set; } = 0!;
        public string? LATITUDE { get; set; }
        public string? LONGITUDE { get; set; }
        public string? TIPO_SURVEY { get; set; }
        public string? REDE_INTERNA { get; set; }
        public string? UMS_CERTIFICADAS { get; set; }
        public string? REDE_EDIF_CERT { get; set; }
        public int? NUM_PISOS { get; set; }
        public string? DISP_COMERCIAL { get; set; }
        public string? ESTADO_CONTROLE { get; set; }
        public string? DATA_ESTADO_CONTROLE { get; set; }
        public string? ID_CELULA { get; set; }
        public string? QUANTIDADE_HCS { get; set; }
        public string? PROJETO { get; set; }
    }
}
