using SistemaGestionPedidos.Models;
using SistemaGestionPedidos.Models.ViewModels;

namespace SistemaGestionPedidos.Services.Interfaces;

public interface IPedidoService
{
    Task<List<Pedido>> ObtenerTodosAsync();
    Task<Pedido?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Caso de uso principal end-to-end: crea un pedido con sus líneas,
    /// valida stock, calcula el total y descuenta el inventario, todo en una transacción.
    /// </summary>
    Task<Pedido> CrearPedidoAsync(PedidoCreateViewModel modelo);

    Task CambiarEstadoAsync(int pedidoId, EstadoPedido nuevoEstado);
}
