using System.ComponentModel.DataAnnotations;

namespace ControleGestaoFtth.Models
{
    public class Ums
    {
        [Key]
        public int Id { get; set; }
        public int? Quantidade { get; set; }
        public int? EnderecoId { get; set;}
        public Endereco? Enderecos { get; set;}
    }
}
