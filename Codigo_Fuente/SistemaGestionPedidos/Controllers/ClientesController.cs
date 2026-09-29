using Microsoft.AspNetCore.Mvc;
using SistemaGestionPedidos.Exceptions;
using SistemaGestionPedidos.Models;
using SistemaGestionPedidos.Services.Interfaces;

namespace SistemaGestionPedidos.Controllers;

public class ClientesController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _clienteService.ObtenerTodosAsync();
        return View(clientes);
    }

    public async Task<IActionResult> Details(int id)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);
        if (cliente is null) return NotFound();
        return View(cliente);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        if (!ModelState.IsValid) return View(cliente);

        try
        {
            await _clienteService.CrearAsync(cliente);
            TempData["Mensaje"] = "Cliente creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (PedidoInvalidoException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente");
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al crear el cliente.");
            return View(cliente);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);
        if (cliente is null) return NotFound();
        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        if (id != cliente.Id) return BadRequest();
        if (!ModelState.IsValid) return View(cliente);

        try
        {
            await _clienteService.ActualizarAsync(cliente);
            TempData["Mensaje"] = "Cliente actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (EntidadNoEncontradaException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cliente {Id}", id);
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado al actualizar el cliente.");
            return View(cliente);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);
        if (cliente is null) return NotFound();
        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _clienteService.EliminarAsync(id);
            TempData["Mensaje"] = "Cliente eliminado exitosamente.";
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
