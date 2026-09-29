using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SistemaGestionPedidos.Data;
using SistemaGestionPedidos.Services;
using SistemaGestionPedidos.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ---- Servicios MVC ----
builder.Services.AddControllersWithViews();

// ---- Contexto de Base de Datos ----
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---- Inyección de dependencias: Servicios de negocio ----

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

var app = builder.Build();

// ---- Cultura fija ----

var cultura = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;

// ---- Migraciones automaticas al iniciar (útil para el entorno Docker) ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

// ---- Pipeline HTTP ----
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
