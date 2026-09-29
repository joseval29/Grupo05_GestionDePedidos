using Microsoft.EntityFrameworkCore;
using SistemaGestionPedidos.Data;
using SistemaGestionPedidos.Exceptions;
using SistemaGestionPedidos.Models;
using SistemaGestionPedidos.Services.Interfaces;

namespace SistemaGestionPedidos.Services;

public class ProductoService : IProductoService
{
    private readonly ApplicationDbContext _context;

    public ProductoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Producto>> ObtenerTodosAsync() =>
        await _context.Productos.OrderBy(p => p.Nombre).ToListAsync();

    public async Task<Producto?> ObtenerPorIdAsync(int id) =>
        await _context.Productos.FindAsync(id);

    public async Task<Producto> CrearAsync(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return producto;
    }

    public async Task ActualizarAsync(Producto producto)
    {
        var existente = await _context.Productos.FindAsync(producto.Id)
            ?? throw new EntidadNoEncontradaException(nameof(Producto), producto.Id);

        existente.Nombre = producto.Nombre;
        existente.Descripcion = producto.Descripcion;
        existente.Precio = producto.Precio;
        existente.Stock = producto.Stock;

        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.DetallePedidos)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new EntidadNoEncontradaException(nameof(Producto), id);

        if (producto.DetallePedidos.Any())
            throw new PedidoInvalidoException("No se puede eliminar un producto que ya aparece en pedidos.");

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
    }
}
