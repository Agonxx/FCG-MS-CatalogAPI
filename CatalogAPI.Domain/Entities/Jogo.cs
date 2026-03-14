using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Domain.Entities
{
    [Index(nameof(Titulo))]
    public class Jogo
    {
        [Key] public int Id { get; set; }
        [Required][MaxLength(150)] public string Titulo { get; set; }
        [Required][MaxLength(500)] public string Descricao { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Preco { get; set; }
        [Required] public DateTime CadastradoEm { get; set; } = DateTime.UtcNow;
    }
}
