using Microsoft.AspNetCore.Mvc;
using SistemaGestionPedidos.Exceptions;
using SistemaGestionPedidos.Models;
using SistemaGestionPedidos.Services.Interfaces;

namespace SistemaGestionPedidos.Controllers;

public class ProductosController : Controller
{
    private readonly IProductoService _productoService;
    private readonly ILogger<ProductosController> _logger;

    public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
    {
        _productoService = productoService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var productos = await _productoService.ObtenerTodosAsync();
        return View(productos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var producto = await _productoService.ObtenerPorIdAsync(id);
        if (producto is null) return NotFound();
        return View(producto);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (!ModelState.IsValid) return View(producto);

        try
        {
            await _productoService.CrearAsync(producto);
            TempData["Mensaje"] = "Producto creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear producto");
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al crear el producto.");
            return View(producto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _productoService.ObtenerPorIdAsync(id);
        if (producto is null) return NotFound();
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Producto producto)
    {
        if (id != producto.Id) return BadRequest();
        if (!ModelState.IsValid) return View(producto);

        try
        {
            await _productoService.ActualizarAsync(producto);
            TempData["Mensaje"] = "Producto actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (EntidadNoEncontradaException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar producto {Id}", id);
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al actualizar el producto.");
            return View(producto);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _productoService.ObtenerPorIdAsync(id);
        if (producto is null) return NotFound();
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _productoService.EliminarAsync(id);
            TempData["Mensaje"] = "Producto eliminado exitosamente.";
        }
        catch (PedidoInvalidoException ex)
        {
            TempData["Error"] = ex.Message;
        }
        catch (EntidadNoEncontradaException)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
