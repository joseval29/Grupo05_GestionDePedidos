using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionPedidos.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Required]
    [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    // Relación 1 a muchos: un producto puede aparecer en muchos detalles de pedido
    public ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
