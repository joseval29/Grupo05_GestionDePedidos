using SistemaGestionPedidos.Models;

namespace SistemaGestionPedidos.Services.Interfaces;

public interface IProductoService
{
    Task<List<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<Producto> CrearAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(int id);
}
