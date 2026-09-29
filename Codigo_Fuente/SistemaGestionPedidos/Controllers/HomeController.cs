using Microsoft.AspNetCore.Mvc;
using SistemaGestionPedidos.Services.Interfaces;

namespace SistemaGestionPedidos.Controllers;

public class HomeController : Controller
{
    private readonly IClienteService _clienteService;
    private readonly IProductoService _productoService;
    private readonly IPedidoService _pedidoService;

    public HomeController(IClienteService clienteService, IProductoService productoService, IPedidoService pedidoService)
    {
        _clienteService = clienteService;
        _productoService = productoService;
        _pedidoService = pedidoService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalClientes = (await _clienteService.ObtenerTodosAsync()).Count;
        ViewBag.TotalProductos = (await _productoService.ObtenerTodosAsync()).Count;
        ViewBag.TotalPedidos = (await _pedidoService.ObtenerTodosAsync()).Count;
        return View();
    }

    public IActionResult Error() => View();
}
