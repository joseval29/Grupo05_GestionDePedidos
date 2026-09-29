using Microsoft.EntityFrameworkCore;
using SistemaGestionPedidos.Models;

namespace SistemaGestionPedidos.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallePedidos => Set<DetallePedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cliente - Pedido (1 a muchos)
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict); // no borrar cliente si tiene pedidos

        // Pedido - DetallePedido (1 a muchos)
        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Pedido)
            .WithMany(p => p.DetallePedidos)
            .HasForeignKey(d => d.PedidoId)
            .OnDelete(DeleteBehavior.Cascade); // si se borra el pedido, se borran sus líneas

        // Producto - DetallePedido (1 a muchos)
        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Producto)
            .WithMany(p => p.DetallePedidos)
            .HasForeignKey(d => d.ProductoId)
            .OnDelete(DeleteBehavior.Restrict); // no borrar producto si aparece en pedidos

        // Índices útiles
        modelBuilder.Entity<Cliente>().HasIndex(c => c.Email).IsUnique();
    }
}
