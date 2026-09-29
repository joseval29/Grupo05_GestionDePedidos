using Microsoft.AspNetCore.Mvc;
using SistemaGestionPedidos.Exceptions;
using SistemaGestionPedidos.Models;
using SistemaGestionPedidos.Models.ViewModels;
using SistemaGestionPedidos.Services.Interfaces;

namespace SistemaGestionPedidos.Controllers;

public class PedidosController : Controller
{
    private readonly IPedidoService _pedidoService;
    private readonly IClienteService _clienteService;
    private readonly IProductoService _productoService;
    private readonly ILogger<PedidosController> _logger;

    public PedidosController(
        IPedidoService pedidoService,
        IClienteService clienteService,
        IProductoService productoService,
        ILogger<PedidosController> logger)
    {
        _pedidoService = pedidoService;
        _clienteService = clienteService;
        _productoService = productoService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var pedidos = await _pedidoService.ObtenerTodosAsync();
        return View(pedidos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var pedido = await _pedidoService.ObtenerPorIdAsync(id);
        if (pedido is null) return NotFound();
        return View(pedido);
    }

    // GET: /Pedidos/Create -> muestra el formulario con clientes y productos disponibles
    public async Task<IActionResult> Create()
    {
        var modelo = new PedidoCreateViewModel
        {
            ClientesDisponibles = await _clienteService.ObtenerTodosAsync(),
            ProductosDisponibles = await _productoService.ObtenerTodosAsync(),
            Lineas = new List<LineaPedidoViewModel> { new LineaPedidoViewModel() } // una línea vacía inicial
        };
        return View(modelo);
    }

    // POST: /Pedidos/Create -> ejecuta el caso de uso end-to-end completo
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PedidoCreateViewModel modelo)
    {
        // Quitar líneas vacías que el usuario no llenó
        modelo.Lineas = modelo.Lineas?.Where(l => l.ProductoId != 0 && l.Cantidad > 0).ToList() ?? new();

        // Las líneas descartadas dejaron errores de binding en el ModelState (p. ej. ProductoId vacío);
        // se eliminan para que no bloqueen el guardado. La validez de las líneas se revisa arriba y en el Service.
        foreach (var clave in ModelState.Keys.Where(k => k.StartsWith(nameof(modelo.Lineas))).ToList())
        {
            ModelState.Remove(clave);
        }

        if (!ModelState.IsValid || !modelo.Lineas.Any())
        {
            if (!modelo.Lineas.Any())
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un producto válido al pedido.");

            modelo.ClientesDisponibles = await _clienteService.ObtenerTodosAsync();
            modelo.ProductosDisponibles = await _productoService.ObtenerTodosAsync();
            return View(modelo);
        }

        try
        {
            var pedido = await _pedidoService.CrearPedidoAsync(modelo);
            TempData["Mensaje"] = $"Pedido #{pedido.Id} creado exitosamente. Total: {pedido.Total:C}";
            return RedirectToAction(nameof(Details), new { id = pedido.Id });
        }
        catch (StockInsuficienteException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (EntidadNoEncontradaException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (PedidoInvalidoException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al crear el pedido");
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al procesar el pedido. Intente nuevamente.");
        }

        modelo.ClientesDisponibles = await _clienteService.ObtenerTodosAsync();
        modelo.ProductosDisponibles = await _productoService.ObtenerTodosAsync();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int id, EstadoPedido nuevoEstado)
    {
        try
        {
            await _pedidoService.CambiarEstadoAsync(id, nuevoEstado);
            TempData["Mensaje"] = "Estado del pedido actualizado.";
        }
        catch (EntidadNoEncontradaException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id });
    }
}
