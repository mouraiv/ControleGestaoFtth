﻿using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Estacoe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Sigla { get; set; }
        [Required]
        public string? NomeEstacao { get; set; }
        [Required]
        public string? Responsavel { get; set; }
    }
}
