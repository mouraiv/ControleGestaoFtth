﻿using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Materiais
    {
        [Key]
        public int Id { get; set; }
        public string? SiglaFederativa { get; set; }
        public string NomeUnidFederativa { get; set; } = null!;
        public string Municipio { get; set; } = null!;
        public string? SiglaLocalidade { get; set; }
        public string? NomeLocalidade { get; set; }
        public string? NomeEstAbastecedora { get; set; }
        public string? SiglaEstAbastecedora { get; set; }
        public string Infraestrutura { get; set; } = null!;
        public string? Codigo { get; set; }
        public string? ElementoRede { get; set; }
        public string? Tipo { get; set; }
        public string? Fabricante { get; set; }
        public string? Modelo { get; set; }
        public string? CodigoSAP { get; set; }
        public string? EstadoOperacional { get; set; }
        public string? DataEstadoOperacional { get; set; }
        public string? LoginUsuario { get; set; }
        public string? Endereco { get; set; }
        public string? GrupoOperacional { get; set; }
        public string? EstadoControle { get; set; }
        public string? DataEstadoControle { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
