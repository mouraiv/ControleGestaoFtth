﻿using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Regioe
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public virtual ICollection<Estado> Estados { get; set; } = null!;
    }
}
