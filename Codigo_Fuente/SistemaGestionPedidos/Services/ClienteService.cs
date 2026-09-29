using Microsoft.EntityFrameworkCore;
using SistemaGestionPedidos.Data;
using SistemaGestionPedidos.Exceptions;
using SistemaGestionPedidos.Models;
using SistemaGestionPedidos.Services.Interfaces;

namespace SistemaGestionPedidos.Services;

public class ClienteService : IClienteService
{
    private readonly ApplicationDbContext _context;

    public ClienteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> ObtenerTodosAsync() =>
        await _context.Clientes.OrderBy(c => c.Nombre).ToListAsync();

    public async Task<Cliente?> ObtenerPorIdAsync(int id) =>
        await _context.Clientes.FindAsync(id);

    public async Task<Cliente> CrearAsync(Cliente cliente)
    {
        var correoExiste = await _context.Clientes.AnyAsync(c => c.Email == cliente.Email);
        if (correoExiste)
            throw new PedidoInvalidoException($"Ya existe un cliente registrado con el correo '{cliente.Email}'.");

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task ActualizarAsync(Cliente cliente)
    {
        var existente = await _context.Clientes.FindAsync(cliente.Id)
            ?? throw new EntidadNoEncontradaException(nameof(Cliente), cliente.Id);

        existente.Nombre = cliente.Nombre;
        existente.Email = cliente.Email;
        existente.Telefono = cliente.Telefono;

        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new EntidadNoEncontradaException(nameof(Cliente), id);

        if (cliente.Pedidos.Any())
            throw new PedidoInvalidoException("No se puede eliminar un cliente con pedidos registrados.");

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
    }
}
