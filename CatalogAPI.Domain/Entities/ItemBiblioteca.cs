using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogAPI.Domain.Entities
{
    [Index(nameof(UsuarioId), nameof(JogoId), IsUnique = true)]
    public class ItemBiblioteca
    {
        [Key] public int Id { get; set; }
        [Required] public int UsuarioId { get; set; }
        [Required] public int JogoId { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal PrecoPago { get; set; }
        [Required] public DateTime AdquiridoEm { get; set; } = DateTime.UtcNow;
    }
}
