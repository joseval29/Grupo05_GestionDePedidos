using Microsoft.EntityFrameworkCore;
using SistemaGestionPedidos.Data;
using SistemaGestionPedidos.Exceptions;
using SistemaGestionPedidos.Models;
using SistemaGestionPedidos.Models.ViewModels;
using SistemaGestionPedidos.Services.Interfaces;

namespace SistemaGestionPedidos.Services;

public class PedidoService : IPedidoService
{
    private readonly ApplicationDbContext _context;

    public PedidoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pedido>> ObtenerTodosAsync() =>
        await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.DetallePedidos)
                .ThenInclude(d => d.Producto)
            .OrderByDescending(p => p.FechaPedido)
            .ToListAsync();

    public async Task<Pedido?> ObtenerPorIdAsync(int id) =>
        await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.DetallePedidos)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

    /// <summary>
    /// Caso de uso end-to-end:
    /// 1) Valida que el cliente exista.
    /// 2) Valida que cada producto exista y tenga stock suficiente.
    /// 3) Crea el pedido y sus líneas (DetallePedido).
    /// 4) Descuenta el stock de cada producto.
    /// 5) Calcula el total.
    /// 6) Persiste todo en una única transacción (atomicidad — ventaja típica del monolito).
    /// </summary>
    public async Task<Pedido> CrearPedidoAsync(PedidoCreateViewModel modelo)
    {
        if (modelo.Lineas is null || !modelo.Lineas.Any())
            throw new PedidoInvalidoException("El pedido debe tener al menos un producto.");

        var cliente = await _context.Clientes.FindAsync(modelo.ClienteId)
            ?? throw new EntidadNoEncontradaException(nameof(Cliente), modelo.ClienteId);

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                FechaPedido = DateTime.UtcNow,
                Estado = EstadoPedido.Pendiente,
                DetallePedidos = new List<DetallePedido>()
            };

            decimal total = 0m;

            foreach (var linea in modelo.Lineas)
            {
                var producto = await _context.Productos.FindAsync(linea.ProductoId)
                    ?? throw new EntidadNoEncontradaException(nameof(Producto), linea.ProductoId);

                if (producto.Stock < linea.Cantidad)
                    throw new StockInsuficienteException(producto.Nombre, producto.Stock, linea.Cantidad);

                // Descontar stock (regla de negocio central del flujo)
                producto.Stock -= linea.Cantidad;

                var detalle = new DetallePedido
                {
                    ProductoId = producto.Id,
                    Cantidad = linea.Cantidad,
                    PrecioUnitario = producto.Precio
                };

                pedido.DetallePedidos.Add(detalle);
                total += detalle.Cantidad * detalle.PrecioUnitario;
            }

            pedido.Total = total;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return pedido;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task CambiarEstadoAsync(int pedidoId, EstadoPedido nuevoEstado)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId)
            ?? throw new EntidadNoEncontradaException(nameof(Pedido), pedidoId);

        pedido.Estado = nuevoEstado;
        await _context.SaveChangesAsync();
    }
}
