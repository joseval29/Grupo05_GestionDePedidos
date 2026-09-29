using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionPedidos.Models;

// Entidad puente entre Pedido y Producto (relación muchos a muchos con datos propios:
// cantidad y precio unitario en el momento de la compra).
public class DetallePedido
{
    public int Id { get; set; }

    [Required]
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    [Required]
    [Display(Name = "Producto")]
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
    public int Cantidad { get; set; }

    // Se guarda el precio al momento de la compra (no se recalcula si el producto cambia de precio después)
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Precio unitario")]
    public decimal PrecioUnitario { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal => Cantidad * PrecioUnitario;
}
