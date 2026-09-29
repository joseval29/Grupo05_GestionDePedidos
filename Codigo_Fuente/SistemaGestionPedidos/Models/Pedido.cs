using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionPedidos.Models;

public class Pedido
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Cliente")]
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    [Display(Name = "Fecha del pedido")]
    public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

    [Display(Name = "Estado")]
    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }

    // Relación 1 a muchos: un pedido tiene muchos detalles (líneas de producto)
    public ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();
}
