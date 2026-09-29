using SistemaGestionPedidos.Models;

namespace SistemaGestionPedidos.Data;

// Carga datos de ejemplo al iniciar el contenedor, para que el profesor pueda
// probar el flujo end-to-end sin tener que registrar clientes/productos manualmente.
public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Clientes.Any())
        {
            context.Clientes.AddRange(
                new Cliente { Nombre = "Ana Torres", Email = "ana.torres@example.com", Telefono = "70012345", FechaRegistro = DateTime.UtcNow },
                new Cliente { Nombre = "Luis Fernández", Email = "luis.fernandez@example.com", Telefono = "70054321", FechaRegistro = DateTime.UtcNow }
            );
        }

        if (!context.Productos.Any())
        {
            context.Productos.AddRange(
                new Producto { Nombre = "Teclado mecánico", Descripcion = "Switches rojos, retroiluminado", Precio = 45.99m, Stock = 30 },
                new Producto { Nombre = "Mouse inalámbrico", Descripcion = "2.4GHz, ergonómico", Precio = 19.50m, Stock = 50 },
                new Producto { Nombre = "Monitor 24\"", Descripcion = "Full HD, 75Hz", Precio = 129.00m, Stock = 15 }
            );
        }

        context.SaveChanges();
    }
}
