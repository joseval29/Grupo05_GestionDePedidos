using System.ComponentModel.DataAnnotations;

namespace SistemaGestionPedidos.Models.ViewModels;

public class PedidoCreateViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un cliente.")]
    [Display(Name = "Cliente")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Debe agregar al menos un producto.")]
    [MinLength(1, ErrorMessage = "Debe agregar al menos un producto al pedido.")]
    public List<LineaPedidoViewModel> Lineas { get; set; } = new();

    // Se llenan en el Controller para poblar los <select> de la vista
    public List<Cliente> ClientesDisponibles { get; set; } = new();
    public List<Producto> ProductosDisponibles { get; set; } = new();
}

public class LineaPedidoViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un producto.")]
    public int ProductoId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
    public int Cantidad { get; set; }
}
