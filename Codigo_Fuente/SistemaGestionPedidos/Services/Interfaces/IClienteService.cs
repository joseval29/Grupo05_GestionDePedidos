using SistemaGestionPedidos.Models;

namespace SistemaGestionPedidos.Services.Interfaces;

public interface IClienteService
{
    Task<List<Cliente>> ObtenerTodosAsync();
    Task<Cliente?> ObtenerPorIdAsync(int id);
    Task<Cliente> CrearAsync(Cliente cliente);
    Task ActualizarAsync(Cliente cliente);
    Task EliminarAsync(int id);
}
